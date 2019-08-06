var url = config.api.events;
var datatable = $('#grid').sigmaDataTable({
    buttons: [
    ],
    columns: [
        { data: "Id", text: 'شناسه', width: '90px' },
        { data: "User", text: 'کاربر', render(row) { return row["User"].Name } },
        { data: "Message", text: 'پیام' },
        { data: "JsonInfo", text: 'اطلاعات مازاد' },
        { data: "Type", text: 'نوع' },
        { data: "Level", text: 'سطح رویداد' },
        {
            data: "Status", text: 'وضعیت', width: '150px', type: 'select',
            render(row) { return EventStatuses[row.Status] ? EventStatuses[row.Status].Value : '-' },
            filter: {
                items: EventStatuses
            }
        },
        {
            text: '', width: '106px', type: 'control', render(row) {
                return `<div class="controls">
                                                                <a class="btn btn-orange" href="#detail/${row.Id}"><i class="mdi mdi-pencil-off"></i></a>
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
            data: { model: new VM_Event(), get StatusName() { return this.model.GetStatusName(); } }
        });
    }
    $.get(url + args[0]).then((res) => this.app.model = new VM_Event(res));
}