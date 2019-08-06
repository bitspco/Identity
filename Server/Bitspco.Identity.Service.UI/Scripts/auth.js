$(document).ready(function () {
    var app = new Vue({
        el: `body form`,
        data: { progress: false, model: new VM_Login(), message: null },
        methods: {
            submit() {
                this.progress = true;
                $.post(config.api.auth, JSON.stringify(this.model)).then(res => {
                    if (res.Success) {
                        config.loginInfo = res.Data;
                        location.href = '/';
                    } else {
                        this.message = res.Message;
                        this.model = new VM_Login();
                        $('input').first().focus();
                    }
                }).always(() => { this.progress = false; });
            }
        }
    });
    $('input').first().focus();
});