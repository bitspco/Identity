using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Common
{
    public class PolicyExperession
    {
        enum Operation { None = 0, And = 1, Or = 2, Not = 3 }
        class Node
        {
            public string Value { get; set; }
            public Operation Operation { get; set; } = Operation.None;
            public List<Node> Childrens { get; set; } = new List<Node>();

            public bool Foreach(Func<string, bool> func)
            {
                if (Childrens.Count > 0)
                {
                    bool result = false;
                    foreach (var item in Childrens)
                    {
                        switch (item.Operation)
                        {
                            case Operation.None:
                                result = item.Foreach(func);
                                break;
                            case Operation.And:
                                result = result && item.Foreach(func);
                                break;
                            case Operation.Or:
                                if (result) return true;
                                result = result || item.Foreach(func);
                                break;
                            case Operation.Not:
                                result = !result;
                                break;
                        }
                    }
                    return result;
                }
                else
                {
                    return func(Value);
                }
            }
        }
        public List<string> _permissions;
        public List<string> _roles;
        public Dictionary<string, string> _claims;
        public List<string> Messages { get; set; }
        private Node Root { get; set; } = new Node();
        private static Dictionary<char, Operation> OperationDictionary = new Dictionary<char, Operation>();
        static PolicyExperession()
        {
            OperationDictionary.Add('&', Operation.And);
            OperationDictionary.Add('|', Operation.Or);
            OperationDictionary.Add('!', Operation.Not);
        }

        public PolicyExperession(List<string> roles, List<string> permissions, Dictionary<string, string> claims)
        {
            _roles = roles;
            _permissions = permissions;
            _claims = claims;
        }
        private Node Parse(string policy)
        {
            var parent = new Node();
            bool flag = false;
            Operation op = Operation.None;
            int pCount = 0, pStart = -1;
            string statement = "";
            for (int i = 0; i < policy.Length; i++)
            {
                if (OperationDictionary.ContainsKey(policy[i]))
                {
                    op = OperationDictionary[policy[i]];
                    if (statement.Length > 0 && flag == false)
                    {
                        parent.Childrens.Add(new Node()
                        {
                            Value = statement,
                            Operation = op
                        });
                    }
                    statement = "";
                }
                else if (policy[i] == '(')
                {
                    pCount++;
                    if (flag == false) { pStart = i + 1; flag = true; }
                    statement = "";
                }
                else if (policy[i] == ')')
                {
                    pCount--;
                    if (pCount == 0)
                    {
                        if (flag)
                        {
                            flag = false;
                            var node = Parse(policy.Substring(pStart, i - 1));
                            node.Operation = op;
                            parent.Childrens.Add(node);
                        }
                        else throw new Exception("Ilegal Permission Format");
                    }
                    statement = "";
                }
                else statement += policy[i];
            }
            if (statement.Length > 0 && flag == false)
            {
                parent.Childrens.Add(new Node()
                {
                    Value = statement,
                    Operation = op
                });
            }
            return parent;
        }
        public bool HasPolicy(string policy)
        {
            if (policy == null) return true;
            Messages = new List<string>();
            var result = Parse(policy).Foreach(value =>
            {
                try
                {
                    return CheckPolicy(value);
                }
                catch (Exception ex)
                {
                    Messages.Add(ex.Message);
                    return false;
                }
            });
            if (!result) return false;
            return true;
        }
        public T GetInformation<T>(string symbol)
        {
            var value = _claims.FirstOrDefault(x => x.Key == symbol);
            if (value.Key != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value.Value);
                return result;
            }
            return default(T);
        }

        private bool CheckPolicy(string policy)
        {
            if (policy?.Length < 1) return true;
            if (policy?.Length > 2 && policy[1] == ':')
            {
                var key = policy.Substring(0, 2);
                policy = policy.Substring(2);
                switch (key)
                {
                    case "c:":
                        if (policy.IndexOf('=') > -1)
                        {
                            var split = policy.Split('=');
                            if (GetInformation<object>(split[0]).ToString() == split[1].Trim()) return true;
                            throw new Exception($"Claim '{policy}' Don't have valid data.");
                        }
                        if (_claims.ContainsKey(policy)) return true;
                        throw new Exception($"Claim '{policy}' Not Found!!!");
                    case "r:":
                        if (_roles.Exists(x => x == policy)) return true;
                        throw new Exception($"Role '{policy}' Not Found!!!");
                    case "p:":
                        if (_permissions.Exists(x => x == policy)) return true;
                        throw new Exception($"Permission '{policy}' Not Found!!!");
                }
            }
            if (_permissions?.Exists(x => x == policy) == true) return true;
            throw new Exception($"Permission '{policy}' Not Found!!!");
        }
    }
}
