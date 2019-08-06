var url = config.api.thirdPartyAccesses;
var datatable = $('#grid').sigmaDataTable({
    buttons: [
        { render: `<a class="btn btn-green-dark" href="#create">افزودن</a>` }
    ],
    columns: [
        { data: "Id", text: 'شناسه', width: '90px' },
        { data: "Module", text: 'ماژول', render(row) { return row.Module ? row.Module.Name : '-'; } },
        { data: "Name", text: 'نام' },
        { data: "Symbol", text: 'نماد' },
        { data: "Description", text: 'توضیحات' },
        { data: "Icon", text: 'آیکون' },
        {
            data: "Status", text: 'وضعیت', width: '150px', type: 'select',
            render(row) { return ThirdPartyAccessStatuses[row.Status] ? ThirdPartyAccessStatuses[row.Status].Value : '-' },
            filter: {
                items: ThirdPartyAccessStatuses
            }
        },
        {
            text: '', width: '136px', type: 'control', render(row) {
                return `<div class="controls">
                                                                    <a class="btn btn-black" href="#apps/${row.Id}"><i class="mdi mdi-application"></i></a>
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
            data: { modules: [], model: new VM_ThirdPartyAccess() },
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
                $.get(config.api.modules).then(res => this.modules = ConvertArrayItems(res, VM_Module));
            }
        });
    }
    this.app.model = new VM_ThirdPartyAccess();
}
document.getElementById('detail').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_ThirdPartyAccess(), get StatusName() { return this.model.GetStatusName(); } }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_ThirdPartyAccess(res));
}
document.getElementById('edit').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_ThirdPartyAccess() },
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_ThirdPartyAccess(res));
}
document.getElementById('delete').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { model: new VM_ThirdPartyAccess(), get StatusName() { return this.model.GetStatusName(); } },
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
    $.get(url + args[0]).then((res) => this.app.model = new VM_ThirdPartyAccess(res));
}
document.getElementById('apps').onshow = function (args) {
    if (!this.app) {
        this.app = new Vue({
            el: `#${this.id} .body`,
            data: { items: [] }
        });
    }
    $.get(url + args[0] + '/Apps').then((res) => this.app.items = ConvertArrayItems(res, VM_ThirdPartyAppAccess));
}