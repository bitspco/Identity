var url = config.api.tokens;
var datatable = $('#grid').sigmaDataTable({
    buttons: [
    ],
    columns: [
        { data: "Id", text: 'شناسه', width: '90px' },
        { data: "User", text: 'کاربر', render(row) { return row.User ? row.User.Name : '-'; } },
        { data: "CreationTime", text: 'زمان ایجاد' },
        { data: "ExpireTime", text: 'زمان انقضا' },
        { data: "VerificationCode", text: 'کد تاییدیه', render(row) { return row.VerificationCode ? row.VerificationCode : '-'; } },
        { data: "VerificationTime", text: 'زمان تایید', render(row) { return row.VerificationTime ? row.VerificationTime : '-'; } },
        { data: "IsNeedVerification", text: 'نیازمند تاییدیه', width: '150px', render(row) { return row.IsNeedVerification ? 'بله' : 'خیر'; } },
        {
            data: "Status", text: 'وضعیت', width: '150px', type: 'select',
            render(row) { return TokenStatuses[row.Status] ? TokenStatuses[row.Status].Value : '-' },
            filter: {
                items: TokenStatuses
            }
        },
        {
            text: '', width: '106px', type: 'control', render(row) {
                return `<div class="controls">
                                                                    <a class="btn btn-orange" href="#detail/${row.Id}"><i class="mdi mdi-pencil-off"></i></a>
                                                                    <a class="btn btn-blue-dark" href="#expire/${row.Id}"><i class="mdi mdi-sleep"></i></a>
                                                                    <a class="btn btn-red-dark" href="#delete/${row.Id}"><i class="mdi mdi-delete"></i></a>
                                                                </div>`;
            }
        }
    ],
    data: {
        ajax: url,
    },
});
document.getElementById('detail').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Token(), get StatusName() { return this.model.GetStatusName(); } }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_Token(res));
}
document.getElementById('expire').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Token(), get StatusName() { return this.model.GetStatusName(); } },
            methods: {
                submit() {
                    $.ajax({ url: url + this.model.Id + '/Expire', method: 'PUT' }).then(res => {
                        if (res.Success) {
                            datatable.load();
                            Swal.fire('موفقیت', 'انقضای توکن با موفقیت انجام شد', 'success').then(() => {
                                location.hash = '';
                            });
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                }
            }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_Token(res));
}
document.getElementById('delete').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Token(), get StatusName() { return this.model.GetStatusName(); } },
            methods: {
                submit() {
                    $.ajax({ url: url + this.model.Id, method: 'DELETE' }).then(res => {
                        if (res.Success) {
                            datatable.load();
                            Swal.fire('موفقیت', 'حذف ردیف با موفقیت انجام شد', 'success').then(() => {
                                location.hash = '';
                            });
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                }
            }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_Token(res));
}