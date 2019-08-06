var url = config.api.roles;
var datatable = $('#grid').sigmaDataTable({
    buttons: [
        { render: `<a class="btn btn-green-dark" href="#create">افزودن</a>` }
    ],
    columns: [
        { data: "Id", text: 'شناسه', width: '90px' },
        { data: "Module", text: 'ماژول', render(row) { return row.Module ? row.Module.Name : '-'; } },
        { data: "Name", text: 'نام' },
        { data: "Symbol", text: 'نماد' },
        {
            data: "Status", text: 'وضعیت', width: '150px', type: 'select',
            render(row) { return RoleStatuses[row.Status] ? RoleStatuses[row.Status].Value : '-' },
            filter: {
                items: RoleStatuses
            }
        },
        {
            text: '', width: '227px', type: 'control', render(row) {
                return `<div class="controls">
                                                                                <a class="btn btn-green-dark" href="#users/${row.Id}"><i class="mdi mdi-account-multiple-outline"></i></a>
                                                                                <a class="btn btn-yellow-dark" href="#permissions/${row.Id}"><i class="mdi mdi-key-variant"></i></a>
                                                                                <a class="btn btn-green-dark" href="#members/${row.Id}"><i class="mdi mdi-file-tree"></i></a>
                                                                                <a class="btn btn-violet-dark" href="#parents/${row.Id}"><i class="mdi mdi-file-tree"></i></a>
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
            data: { modules: [], model: new VM_Role() },
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
    this.app.model = new VM_Role();
}
document.getElementById('detail').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Role(), get StatusName() { return this.model.GetStatusName(); } }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_Role(res));
}
document.getElementById('edit').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Role() },
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_Role(res));
}
document.getElementById('delete').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_Role(), get StatusName() { return this.model.GetStatusName(); } },
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_Role(res));
}
document.getElementById('users').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { users: [], id: null, model: new VM_UserRole(), items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Users`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Users').then((res) => this.items = ConvertArrayItems(res, VM_UserRole));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Users/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Users').then((res) => this.items = ConvertArrayItems(res, VM_UserRole));
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
    $.get(url + args[0] + '/Users').then((res) => this.app.items = ConvertArrayItems(res, VM_UserRole));
}
document.getElementById('permissions').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, permissions: [], model: new VM_RolePermission(), items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Permissions`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Permissions').then((res) => this.items = ConvertArrayItems(res, VM_RolePermission));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Permissions/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Permissions').then((res) => this.items = ConvertArrayItems(res, VM_RolePermission));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                $.get(config.api.permissions).then(res => this.permissions = ConvertArrayItems(res, VM_Permission));
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Permissions').then((res) => this.app.items = ConvertArrayItems(res, VM_RolePermission));
}
document.getElementById('members').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { roles: [], id: null, model: new VM_RoleMember(), items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Members`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Members').then((res) => this.items = ConvertArrayItems(res, VM_RoleMember));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Members/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Members').then((res) => this.items = ConvertArrayItems(res, VM_RoleMember));
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
    $.get(url + args[0] + '/Members').then((res) => this.app.items = ConvertArrayItems(res, VM_RoleMember));
}
document.getElementById('parents').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { roles: [], id: null, model: new VM_RoleMember(), items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Parents`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Parents').then((res) => this.items = ConvertArrayItems(res, VM_RoleMember));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Parents/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Parents').then((res) => this.items = ConvertArrayItems(res, VM_RoleMember));
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
    $.get(url + args[0] + '/Parents').then((res) => this.app.items = ConvertArrayItems(res, VM_RoleMember));
}