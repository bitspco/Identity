$(document).ready(function () {
    var app = new Vue({
        el: `body form`,
        data: { model: new VM_Login(), message: null },
        methods: {
            submit() {
                $.post(config.api.auth, JSON.stringify(this.model)).then(res => {
                    if (res.Success) {
                        config.loginInfo = res.Data;
                        location.href = '/';
                    } else {
                        this.message = res.Message;
                        this.newUser();
                        $('input').first().focus();
                    }
                });
            },
            newUser() {
                this.model = new VM_Login();
                var loginInfo = config.loginInfo;
                console.log(loginInfo);
                if (loginInfo && loginInfo.Username) this.model.Username = loginInfo.Username;
                else location.href = '/Auth'
            }
        },
        created() {
            this.newUser();
        }
    });
    $('input').first().focus();
});