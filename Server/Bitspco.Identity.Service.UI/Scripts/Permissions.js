var url = config.api.permissions;
var datatable = $('#grid').sigmaDataTable({
    buttons: [
        { render: `<a class="btn btn-green-dark" href="#create">افزودن</a>` }
    ],
    columns: [
        { data: "Id", text: 'شناسه', width: '90px' },
        { data: "Name", text: 'نام' },
        { data: "Symbol", text: 'نماد' },
        {
            data: "Status", text: 'وضعیت', width: '150px', type: 'select',
            render(row) { return PermissionStatuses[row.Status] ? PermissionStatuses[row.Status].Value : '-' },
            filter: {
                items: PermissionStatuses
            }
        },
        {
            text: '', width: '167px', type: 'control', render(row) {
                return `<div class="controls">
                                                                        <a class="btn btn-green-dark" href="#roles/${row.Id}"><i class="mdi mdi-chemical-weapon"></i></a>
                                                                        <a class="btn btn-violet-dark" href="#users/${row.Id}"><i class="mdi mdi-account-multiple-outline"></i></a>
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
            data: { modules: [], model: new VM_Permission() },
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
            },
            created() {
                $.get(config.api.modules).then(res => this.modules = ConvertArrayItems(res, VM_Module))
            }
        });
    }
    this.app.model = new VM_Permission();
}
document.getElementById('detail').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Permission(), get StatusName() { return this.model.GetStatusName(); } }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_Event(res));
}
document.getElementById('edit').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Permission() },
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_Event(res));
}
document.getElementById('delete').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Permission(), get StatusName() { return this.model.GetStatusName(); } },
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_Event(res));
}
document.getElementById('roles').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { roles: [], id: null, model: new VM_RolePermission(), items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Roles`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Roles').then((res) => this.items = ConvertArrayItems(res, VM_RolePermission));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Roles/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Roles').then((res) => this.items = ConvertArrayItems(res, VM_RolePermission));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                $.get(config.api.roles).then(res => this.roles = ConvertArrayItems(res, VM_Role));
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Roles').then((res) => this.app.items = ConvertArrayItems(res, VM_RolePermission));
}
document.getElementById('users').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { users: [], id: null, model: new VM_UserPermission(), items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Users`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Users').then((res) => this.items = ConvertArrayItems(res, VM_UserPermission));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.permissionId}/Users/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Users').then((res) => this.items = ConvertArrayItems(res, VM_UserPermission));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                $.get(config.api.users).then(res => this.users = ConvertArrayItems(res, VM_User));
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Users').then((res) => this.app.items = ConvertArrayItems(res, VM_UserPermission));
}