var url = config.api.users;
var datatable = $('#grid').sigmaDataTable({
    buttons: [
        { render: `<a class="btn btn-green-dark" href="#create">افزودن</a>` }
    ],
    columns: [
        { data: "Id", text: 'شناسه', width: '90px' },
        { data: "Name", text: 'نام' },
        { data: "Username", text: 'نام کاربری' },
        { data: "NationalId", text: 'کد ملی', render(row) { return row.NationalId ? row.NationalId : '-'; } },
        { data: "Gender", text: 'جنسیت', render(row) { return Genders[row.Gender] ? Genders[row.Gender].Value : '-'; } },
        { data: "Position", text: 'موقعیت', render(row) { return row.Position ? row.Position.Name : '-'; } },
        { data: "Description", text: 'توضیحات', render(row) { return row.Description ? row.Description : '-'; } },
        {
            data: "Status", text: 'وضعیت', width: '150px', type: 'select',
            render(row) { return UserStatuses[row.Status] ? UserStatuses[row.Status].Value : '-' },
            filter: {
                items: UserStatuses
            }
        },
        {
            text: '', width: '408px', type: 'control', render(row) {
                return `<div class="controls">
                                                                    <a class="btn btn-black" href="#tokens/${row.Id}"><i class="mdi mdi-account-key"></i></a>
                                                                    <a class="btn btn-gray" href="#apps/${row.Id}"><i class="mdi mdi-cellphone-link"></i></a>
                                                                    <a class="btn btn-turquise-dark" href="#events/${row.Id}"><i class="mdi mdi-email-outline"></i></a>
                                                                    <a class="btn btn-red" href="#contacts/${row.Id}"><i class="mdi mdi-contact-mail"></i></a>
                                                                    <a class="btn btn-violet-dark" href="#claims/${row.Id}"><i class="mdi mdi-information-outline"></i></a>
                                                                    <a class="btn btn-green-dark" href="#roles/${row.Id}"><i class="mdi mdi-chemical-weapon"></i></a>
                                                                    <a class="btn btn-yellow-dark" href="#permissions/${row.Id}"><i class="mdi mdi-key-variant"></i></a>
                                                                    <a class="btn btn-sky-dark" href="#questions/${row.Id}"><i class="mdi mdi-help-circle-outline"></i></a>
                                                                    <a class="btn btn-green" href="#members/${row.Id}"><i class="mdi mdi-file-tree"></i></a>
                                                                    <a class="btn btn-violet" href="#parents/${row.Id}"><i class="mdi mdi-file-tree"></i></a>

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
            data: { positions: [], model: new VM_User() },
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
                $.get(config.api.positions).then(res => this.positions = ConvertArrayItems(res, VM_Position));
            }
        });
    }
    this.app.model = new VM_User();
}
document.getElementById('detail').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { positions: [], model: new VM_User(), get StatusName() { return this.model.GetStatusName(); } },
            created() {
                $.get(config.api.positions).then(res => this.positions = ConvertArrayItems(res, VM_Position));
            }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_User(res));
}
document.getElementById('edit').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { positions: [], model: new VM_User() },
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
            },
            created() {
                $.get(config.api.positions).then(res => this.positions = ConvertArrayItems(res, VM_Position));
            }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_User(res));
}
document.getElementById('delete').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_User(), get StatusName() { return this.model.GetStatusName(); } },
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_User(res));
}
document.getElementById('parents').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_UserMember(), users: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Parents`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Parents').then((res) => this.items = ConvertArrayItems(res, VM_UserMember));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Parents/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Parents').then((res) => this.items = ConvertArrayItems(res, VM_UserMember));
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
    $.get(url + args[0] + '/Parents').then((res) => this.app.items = ConvertArrayItems(res, VM_UserMember));
}
document.getElementById('members').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_UserMember(), users: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Members`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Members').then((res) => this.items = ConvertArrayItems(res, VM_UserMember));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Members/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Members').then((res) => this.items = ConvertArrayItems(res, VM_UserMember));
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
    $.get(url + args[0] + '/Members').then((res) => this.app.items = ConvertArrayItems(res, VM_UserMember));
}
document.getElementById('questions').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_UserQuestion(), questions: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Questions`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Questions').then((res) => this.items = ConvertArrayItems(res, VM_UserQuestion));
                            this.model = new VM_UserQuestion();
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Questions/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Questions').then((res) => this.items = ConvertArrayItems(res, VM_UserQuestion));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                $.get(config.api.questions).then(res => this.questions = ConvertArrayItems(res, VM_Question));
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Questions').then((res) => this.app.items = ConvertArrayItems(res, VM_UserQuestion));
}
document.getElementById('permissions').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_UserPermission(), permissions: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Permissions`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Permissions').then((res) => this.items = ConvertArrayItems(res, VM_UserPermission));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Permissions/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Permissions').then((res) => this.items = ConvertArrayItems(res, VM_UserPermission));
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
    $.get(url + args[0] + '/Permissions').then((res) => this.app.items = ConvertArrayItems(res, VM_UserPermission));
}
document.getElementById('roles').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_UserRole(), roles: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Roles`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Roles').then((res) => this.items = ConvertArrayItems(res, VM_UserRole));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Roles/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Roles').then((res) => this.items = ConvertArrayItems(res, VM_UserRole));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                $.get(config.api.roles).then(res => this.roles = ConvertArrayItems(res, VM_Permission));
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Roles').then((res) => this.app.items = ConvertArrayItems(res, VM_UserRole));
}
document.getElementById('claims').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_UserClaim(), claims: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Claims`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Claims').then((res) => this.items = ConvertArrayItems(res, VM_UserClaim));
                            this.model = new VM_UserClaim();
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Claims/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Claims').then((res) => this.items = ConvertArrayItems(res, VM_UserClaim));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                $.get(config.api.claims).then(res => this.claims = ConvertArrayItems(res, VM_Claim));
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Claims').then((res) => this.app.items = ConvertArrayItems(res, VM_UserClaim));
}
document.getElementById('contacts').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_UserContact(), contacts: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Contacts`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Contacts').then((res) => this.items = ConvertArrayItems(res, VM_UserContact));
                            this.model = new VM_UserContact();
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                approve(id) {
                    $.ajax({ url: `${url + this.id}/Contacts/${id}/Approve`, method: 'PUT' }).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Contacts').then((res) => this.items = ConvertArrayItems(res, VM_UserContact));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                reject(id) {
                    $.ajax({ url: `${url + this.id}/Contacts/${id}/Reject`, method: 'PUT' }).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Contacts').then((res) => this.items = ConvertArrayItems(res, VM_UserContact));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Contacts/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Contacts').then((res) => this.items = ConvertArrayItems(res, VM_UserContact));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                this.contacts = UserContactTypes;
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Contacts').then((res) => this.app.items = ConvertArrayItems(res, VM_UserContact));
}
document.getElementById('events').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_Event(), statuses: [], levels: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Events`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Events').then((res) => this.items = ConvertArrayItems(res, VM_Event));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Events/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Events').then((res) => this.items = ConvertArrayItems(res, VM_Event));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                this.statuses = EventStatuses;
                this.levels = EventLevels;
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Events').then((res) => this.app.items = ConvertArrayItems(res, VM_Event));
}
document.getElementById('apps').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { id: 0, model: new VM_UserApp(), apps: [], items: [] },
            methods: {
                add() {
                    $.post(`${url + this.id}/Apps`, JSON.stringify(this.model)).then(res => {
                        if (res.Success) {
                            $.get(url + args[0] + '/Apps').then((res) => this.items = ConvertArrayItems(res, VM_UserApp));
                        } else {
                            Swal.fire('خطا', res.Message, 'error');
                        }
                    });
                },
                remove(id) {
                    if (confirm('آیا از حذف این ردیف اطمینان دارید ؟'))
                        $.ajax({ url: `${url + this.id}/Apps/${id}`, method: 'DELETE' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Apps').then((res) => this.items = ConvertArrayItems(res, VM_UserApp));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            },
            created() {
                $.get(config.api.authenticatorApps).then(res => this.apps = ConvertArrayItems(res, VM_AuthenticatorApp));
            }
        });
    }
    this.app.id = args[0];
    $.get(url + args[0] + '/Apps').then((res) => this.app.items = ConvertArrayItems(res, VM_UserApp));
}
document.getElementById('tokens').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { items: [] },
            methods: {
                expire(id) {
                    if (confirm('آیا از انقضای این توکن اطمینان دارید ؟'))
                        $.ajax({ url: config.api.tokens + id + '/Expire', method: 'PUT' }).then(res => {
                            if (res.Success) {
                                $.get(url + args[0] + '/Tokens?$orderby=Id desc').then((res) => this.items = ConvertArrayItems(res, VM_Token));
                            } else {
                                Swal.fire('خطا', res.Message, 'error');
                            }
                        });
                }
            }
        });
    }
    $.get(url + args[0] + '/Tokens?$orderby=Id desc').then((res) => this.app.items = ConvertArrayItems(res, VM_Token));
}