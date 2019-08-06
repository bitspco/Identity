var url = config.api.modules;
var datatable = $('#grid').sigmaDataTable({
    buttons: [
        { render: `<a class="btn btn-green-dark" href="#create">افزودن</a>` }
    ],
    columns: [
        { data: "Id", text: 'شناسه', width: '90px' },
        { data: "Name", text: 'نام' },
        { data: "Symbol", text: 'نماد' },
        { data: "Icon", text: 'آیکون' },
        { data: "Link", text: 'لینک سامانه' },
        { data: "Api", text: 'آی پی آی سامانه' },
        {
            data: "Status", text: 'وضعیت', width: '150px', type: 'select',
            render(row) { return ModuleStatuses[row.Status] ? ModuleStatuses[row.Status].Value : '-' },
            filter: {
                items: ModuleStatuses
            }
        },
        {
            text: '', width: '227px', type: 'control', render(row) {
                return `<div class="controls">
                                                                    <a class="btn btn-green-dark" href="#roles/${row.Id}"><i class="mdi mdi-chemical-weapon"></i></a>
                                                                    <a class="btn btn-yellow-dark" href="#permissions/${row.Id}"><i class="mdi mdi-key-variant"></i></a>
                                                                    <a class="btn btn-violet-dark" href="#claims/${row.Id}"><i class="mdi mdi-information-outline"></i></a>
                                                                    <a class="btn btn-gray-dark" href="#thirdPartyAccesses/${row.Id}"><i class="mdi mdi-approval"></i></a>
                                                                    <a class="btn btn-orange" href="#detail/${row.Id}"><i class="mdi mdi-pencil-off"></i></a>
                                                                    <a class="btn btn-blue-dark" href="#edit/${row.Id}"><i class="mdi mdi-lead-pencil"></i></a>
                                                                    <a class="btn btn-red-dark" href="#delete/${row.Id}"><i class="mdi mdi-delete"></i></a>
                                                                </div>`;
            }
        }
    ],
    data: {
        ajax: url,
    },
});
document.getElementById('create').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Module() },
            methods: {
                submit() {
                    $.post(url, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            datatable.load();
                            Swal.fire('موفقیت', 'ثبت ردیف با موفقیت انجام شد', 'success').then(() => {
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
    this.app.model = new VM_Module();
}
document.getElementById('detail').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Module(), get StatusName() { return this.model.GetStatusName(); } }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_Module(res));
}
document.getElementById('edit').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Module() },
            methods: {
                submit() {
                    $.ajax({ url, method: 'PATCH', data: JSON.stringify(this.model) }).then(res => {
                        if (res.Success) {
                            datatable.load();
                            Swal.fire('موفقیت', 'ویرایش ردیف با موفقیت انجام شد', 'success').then(() => {
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_Module(res));
}
document.getElementById('delete').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Module(), get StatusName() { return this.model.GetStatusName(); } },
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_Module(res));
}
document.getElementById('roles').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { items: [] }
        });
    }
    $.get(url + args[0] + '/Roles').then((res) => this.app.items = ConvertArrayItems(res, VM_Role));
}
document.getElementById('permissions').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { items: [] }
        });
    }
    $.get(url + args[0] + '/Permissions').then((res) => this.app.items = ConvertArrayItems(res, VM_Permission));
}
document.getElementById('claims').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { items: [] }
        });
    }
    $.get(url + args[0] + '/Claims').then((res) => this.app.items = ConvertArrayItems(res, VM_Claim));
}
document.getElementById('thirdPartyAccesses').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { items: [] }
        });
    }
    $.get(url + args[0] + '/ThirdPartyAccesses').then((res) => this.app.items = ConvertArrayItems(res, VM_Claim));
}