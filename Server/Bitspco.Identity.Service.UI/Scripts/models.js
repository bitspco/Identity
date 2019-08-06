//====================================== Enums

window.Genders = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'مرد' },
    { Key: 2, Value: 'زن' }
];
window.AuthenticatorAppStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];
window.ClaimStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.ClaimTypes = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'متنی' },
    { Key: 2, Value: 'عددی' },
    { Key: 3, Value: 'دو حالته' },
    { Key: 4, Value: 'تاریخ' },
    { Key: 5, Value: 'زمان' },
    { Key: 6, Value: 'تاریخ و زمان' },
    { Key: 10, Value: 'پیچیده' },
];

window.EventStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.EventLevels = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'اطلاعات' },
    { Key: 2, Value: 'هشدار' },
    { Key: 3, Value: 'خطا' },
];

window.EventTypes = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'اطلاعیه' },
    { Key: 2, Value: 'پیشنهاد' },
    { Key: 3, Value: 'امنیتی' },
];

window.ModuleStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.PermissionStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.QuestionStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.RoleStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.ThirdPartyAccessStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.ThirdPartyAppStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.TokenStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'منقضی شده' },
    { Key: 2, Value: 'غیر فعال' },
    { Key: 3, Value: 'فعال' }
];

window.UserStatuses = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'غیر فعال' },
    { Key: 2, Value: 'فعال' }
];

window.UserContactTypes = [
    { Key: 0, Value: 'نامشخص' },
    { Key: 1, Value: 'تلفن ثابت' },
    { Key: 2, Value: 'تلفن همراه' },
    { Key: 3, Value: 'پست الکترونیکی' },
];

//======================================== View Models
function ConvertArrayItems(array, type) {
    var result = [];
    if (Array.isArray(array)) {
        for (var i = 0; i < array.length; i++) result.push(new type(array[i]));
    }
    return result;
}

function VM_AuthenticatorApp(obj) {
    this.Id = null;
    this.Name = '';
    this.Icon = '';
    this.Status = null;

    this.GetStatusName = function () { return AuthenticatorAppStatuses[this.Status] ? AuthenticatorAppStatuses[this.Status].Value : '-'; }

    return Object.assign(this, obj);
}

function VM_Claim(obj) {
    this.Id = null;
    this.ModuleId = null;
    this.Module = null;
    this.Symbol = '';
    this.Type = null;
    this.Template = '';
    this.Status = null;

    this.GetTypeName = function () { return ClaimTypes[this.Type] ? ClaimTypes[this.Type].Value : '-'; }
    this.GetStatusName = function () { return ClaimStatuses[this.Status] ? ClaimStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    if (this.Module) this.Module = new VM_Module(this.Module);
    return this;
}

function VM_Event(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.Message = '';
    this.JsonInfo = null;
    this.Type = 0;
    this.Level = 0;
    this.Status = null;

    this.GetStatusName = function () { return EventStatuses[this.Status] ? EventStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    return this;
}

function VM_ISP(obj) {
    this.Id = null;
    this.LocationId = null;
    this.Location = null;
    this.Name = '';

    Object.assign(this, obj);
    if (this.Location) this.Location = new VM_Location(this.Location);
    return this;
}

function VM_Location(obj) {
    this.Id = null;
    this.Code = '';
    this.Name = '';
    this.ParentId = null;
    this.Parent = null;

    this.Children = null;

    Object.assign(this, obj);
    if (this.Parent) this.Parent = new VM_Location(this.Parent);
    if (this.Children) this.Children = ConvertArrayItems(this.Children, VM_Location);
    return this;
}

function VM_Module(obj) {
    this.Id = null;
    this.Name = '';
    this.Symbol = '';
    this.Icon = '';
    this.Link = null;
    this.Api = null;
    this.Status = null;

    this.Roles = null;
    this.Permissions = null;
    this.Claims = null;

    this.GetStatusName = function () { return ModuleStatuses[this.Status] ? ModuleStatuses[this.Status].Value : '-'; }

    return Object.assign(this, obj);
}

function VM_Permission(obj) {
    this.Id = null;
    this.Name = '';
    this.Symbol = '';
    this.Status = null;

    this.Users = null;
    this.Roles = null;

    this.GetStatusName = function () { return PermissionStatuses[this.Status] ? PermissionStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    if (this.Users) this.Users = ConvertArrayItems(this.Users, VM_User);
    if (this.Roles) this.Roles = ConvertArrayItems(this.Roles, VM_Role);
    return this;
}

function VM_Position(obj) {
    this.Id = null;
    this.Name = '';
    this.ParentId = null;
    this.Parent = null;

    Object.assign(this, obj);
    if (this.Parent) this.Parent = new VM_Position(this.Parent);
    return this;
}

function VM_Question(obj) {
    this.Id = null;
    this.Text = '';
    this.Status = null;

    this.GetStatusName = function () { return QuestionStatuses[this.Status] ? QuestionStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    return this;
}

function VM_Role(obj) {
    this.Id = null;
    this.ModuleId = null;
    this.Module = null;
    this.Name = '';
    this.Symbol = '';
    this.Status = null;

    this.Users = null;
    this.Permissions = null;
    this.Members = null;
    this.Parents = null;

    this.GetStatusName = function () { return RoleStatuses[this.Status] ? RoleStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    if (this.Module) this.Module = new VM_Module(this.Module);
    if (this.Users) this.Users = ConvertArrayItems(this.Users, VM_User);
    if (this.Permissions) this.Permissions = ConvertArrayItems(this.Permissions, VM_Permission);
    if (this.Members) this.Members = ConvertArrayItems(this.Members, VM_RoleMember);
    if (this.Parents) this.Parents = ConvertArrayItems(this.Parents, VM_RoleMember);
    return this;
}

function VM_RoleMember(obj) {
    this.Id = null;
    this.BaseId = null;
    this.Base = null;
    this.MemberId = null;
    this.Member = null;

    Object.assign(this, obj);
    if (this.Base) this.Base = new VM_Role(this.Base);
    if (this.Member) this.Member = new VM_Permission(this.Member);
    return this;
}

function VM_RolePermission(obj) {
    this.Id = null;
    this.RoleId = null;
    this.Role = null;
    this.PermissionId = null;
    this.Permission = null;

    Object.assign(this, obj);
    if (this.Role) this.Role = new VM_Role(this.Role);
    if (this.Permission) this.Permission = new VM_Permission(this.Permission);
    return this;
}

function VM_ThirdPartyAccess(obj) {
    this.Id = null;
    this.ModuleId = null;
    this.Module = null;
    this.Name = '';
    this.Symbol = '';
    this.Description = '';
    this.Icon = '';
    this.Status = null;

    this.Apps = null;

    this.GetStatusName = function () { return ThirdPartyAccessStatuses[this.Status] ? ThirdPartyAccessStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    if (this.Module) this.Module = new VM_Module(this.Module);
    if (this.Apps) this.Apps = ConvertArrayItems(this.Apps, VM_ThirdPartyApp);
    return this;
}

function VM_ThirdPartyApp(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.Name = '';
    this.Icon = '';
    this.HomePage = '';
    this.AccessGivenTo = '';
    this.AccessGivenTime = null;
    this.Status = null;

    this.Accesses = null;

    this.GetStatusName = function () { return ThirdPartyAppStatuses[this.Status] ? ThirdPartyAppStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    if (this.Accesses) this.Accesses = ConvertArrayItems(this.Accesses, VM_ThirdPartyAccess);
    return this;
}

function VM_ThirdPartyAppAccess(obj) {
    this.Id = null;
    this.ThirdPartyAppId = null;
    this.ThirdPartyApp = null;
    this.ThirdPartyAccessId = null;
    this.ThirdPartyAccess = null;
    this.UserId = null;
    this.User = null;

    Object.assign(this, obj);
    if (this.ThirdPartyApp) this.ThirdPartyApp = new VM_ThirdPartyApp(this.ThirdPartyApp);
    if (this.ThirdPartyAccess) this.ThirdPartyAccess = new VM_ThirdPartyAccess(this.ThirdPartyAccess);
    if (this.User) this.User = new VM_User(this.User);
    return this;
}

function VM_Token(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.Key = '';
    this.CreationTime = null;
    this.ExpireTime = null;
    this.ExpireDescription = '';
    this.VerificationCode = '';
    this.VerificationTime = '';
    this.IsNeedVerification = false;
    this.Status = null;

    this.Usages = null;

    this.GetStatusName = function () { return TokenStatuses[this.Status] ? TokenStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    if (this.Usages) this.Usages = ConvertArrayItems(this.Usages, VM_TokenUsage);
    return this;
}
function VM_TokenUsage(obj) {
    this.Id = null;
    this.TokenId = null;
    this.Token = null;
    this.Location = null;
    this.Ip = null;
    this.Origin = null;
    this.Device = null;
    this.Agent = null;
    this.UserAgentFamily = null;
    this.UserAgentMajor = null;
    this.UserAgentMinor = null;
    this.UserAgentPatch = null;
    this.DeviceBrand = null;
    this.DeviceFamily = null;
    this.DeviceModel = null;
    this.OSFamily = null;
    this.OSMinor = null;
    this.OSMajor = null;
    this.OSPatch = null;
    this.OSPatchMinor = null;
    this.LastRequestTime = null;
    this.CreationTime = null;

    Object.assign(this, obj);
    if (this.Token) this.User = new VM_Token(this.Token);
    return this;
}

function VM_User(obj) {
    this.Id = null;
    this.Name = '';
    this.Username = '';
    this.Password = '';
    this.Image = '';
    this.NationalId = '';
    this.Birthday = '';
    this.Gender = null;
    this.PositionId = null;
    this.Position = null;
    this.Description = '';
    this.Timeout = 30;
    this.MaxTokenCount = 1;
    this.IsNeedChangePassword = false;
    this.IsAppAuthenticatorEnable = false;
    this.IsContactAuthenticatorEnable = false;
    this.Status = null;

    this.Members = null;
    this.Parents = null;
    this.Roles = null;
    this.Permissions = null;
    this.Claims = null;
    this.Tokens = null;
    this.Contacts = null;
    this.Apps = null;
    this.Questions = null;
    this.Events = null;

    this.GetStatusName = function () { return UserStatuses[this.Status] ? UserStatuses[this.Status].Value : '-'; }

    Object.assign(this, obj);
    if (this.Tokens) this.Tokens = ConvertArrayItems(this.Tokens, VM_Token);
    if (this.Events) this.Events = ConvertArrayItems(this.Events, VM_Event);
    if (this.Questions) this.Questions = ConvertArrayItems(this.Questions, VM_UserQuestion);
    if (this.Apps) this.Apps = ConvertArrayItems(this.Apps, VM_UserApp);
    if (this.Contacts) this.Contacts = ConvertArrayItems(this.Contacts, VM_UserContact);
    if (this.Claims) this.Claims = ConvertArrayItems(this.Claims, VM_UserClaim);
    if (this.Roles) this.Roles = ConvertArrayItems(this.Roles, VM_UserRole);
    if (this.Permissions) this.Permissions = ConvertArrayItems(this.Permissions, VM_UserPermission);
    if (this.Members) this.Members = ConvertArrayItems(this.Members, VM_UserMember);
    if (this.Parents) this.Parents = ConvertArrayItems(this.Parents, VM_UserMember);
    return this;
}

function VM_UserApp(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.AuthenticatorAppId = null;
    this.AuthenticatorApp = null;
    this.SecurityKey = '';
    this.Time = '';
    this.IsVerify = false;

    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    if (this.AuthenticatorApp) this.AuthenticatorApp = new VM_AuthenticatorApp(this.AuthenticatorApp);
    return this;
}

function VM_UserClaim(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.ClaimId = null;
    this.Claim = null;
    this.Value = null;

    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    if (this.Claim) this.Claim = new VM_Claim(this.Claim);
    return this;
}

function VM_UserContact(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.Type = null;
    this.Value = null;
    this.IsVerify = false;

    this.GetTypeName = function () { return UserContactTypes[this.Type] ? UserContactTypes[this.Type].Value : '-'; }


    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    return this;
}

function VM_UserMember(obj) {
    this.Id = null;
    this.BaseId = null;
    this.Base = null;
    this.MemberId = null;
    this.Member = null;

    Object.assign(this, obj);
    if (this.Base) this.Base = new VM_User(this.Base);
    if (this.Member) this.Member = new VM_User(this.Member);
    return this;
}

function VM_UserPermission(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.PermissionId = null;
    this.Permission = null;

    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    if (this.Permission) this.Permission = new VM_Permission(this.Permission);
    return this;
}

function VM_UserQuestion(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.QuestionId = null;
    this.Question = null;
    this.Answer = '';

    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    if (this.Question) this.Question = new VM_Question(this.Question);
    return this;
}

function VM_UserRole(obj) {
    this.Id = null;
    this.UserId = null;
    this.User = null;
    this.RoleId = null;
    this.Role = null;

    Object.assign(this, obj);
    if (this.User) this.User = new VM_User(this.User);
    if (this.Role) this.Role = new VM_Role(this.Role);
    return this;
}

function VM_Login() {
    this.Username = '';
    this.Password = '';
}