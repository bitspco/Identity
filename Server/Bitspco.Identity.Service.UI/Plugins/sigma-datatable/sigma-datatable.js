function Observable() {
    var index = 0;
    var _callbacks = [];

    this.subscribe = function (callback) { _callbacks.push(callback); return this; }
    this.next = function () { if (index < _callbacks.length) { _callbacks[index](); index++; } else index = 0; }
    this.emit = function () {
        _callbacks.forEach(function (callback) {
            callback();
        })
    }
}
sigmaDataTableConfiguration = {
    limit: 10,
    rtl: false,
    limits: [10, 20, 50, 100],
};
jQuery.fn.sigmaDataTable = function (config) {
    var element = this[0];
    var datatable = {
        element: element,
        _rtl: (config.rtl === true ? true : (sigmaDataTableConfiguration.rtl ? true : false)),
        _count: 0,
        _page: 1,
        _limit: sigmaDataTableConfiguration.limit,
        _buttons: {
            element: null,
            items: [],
            render: function () {
                this.element.innerHTML = '';
                var top_start_div = document.createElement('div');
                top_start_div.classList.add('sdt-top-start');
                var items = this.items.filter(x => x.direction == 'start' || !x.direction);
                for (var i = 0; i < items.length; i++) {
                    const item = items[i];
                    if (item.render) {
                        var container = document.createElement('div');
                        container.style.order = item.order || 0;
                        var result = item.render;
                        if (typeof result === 'function') result = item.render();
                        if (typeof result === 'string') container.innerHTML = result;
                        else if (result.tagName) container.appendChild(result);
                        top_start_div.appendChild(container);
                    }
                }
                this.element.appendChild(top_start_div);
                var top_end_div = document.createElement('div');
                top_end_div.classList.add('sdt-top-end');
                items = this.items.filter(x => x.direction == 'end');
                for (var i = 0; i < items.length; i++) {
                    const item = items[i];
                    if (item.render) {
                        var container = document.createElement('div');
                        container.style.order = item.order || 0;
                        var result = item.render;
                        if (typeof result === 'function') result = item.render();
                        if (typeof result === 'string') container.innerHTML = result;
                        else if (result.tagName) container.appendChild(result);
                        top_end_div.appendChild(container);
                    }
                }
                this.element.appendChild(top_end_div);
            }
        },
        _limitation: {
            element: null,
            items: sigmaDataTableConfiguration.limits,
            render: function () {
                this.element.innerHTML = '';
                var select = document.createElement('select');
                this.items.forEach(function (item, index) {
                    var option = document.createElement('option');
                    option.value = item;
                    option.innerHTML = item;
                    select.appendChild(option);
                });
                select.addEventListener('change', () => datatable.limit = parseInt(select.value));
                this.element.appendChild(select);
            }
        },
        _controller: {

        },
        _events: {

        },
        _data: {
            element: null,

            _ajax: null,
            get ajax() { return this._ajax; },
            set ajax(value) {
                if (typeof value === "string") {
                    this._ajax = {
                        method: 'GET',
                        url: value,
                        success: function (data) {
                            datatable.data.items = data;
                            datatable.rows.render();
                        }
                    };
                } else this._ajax = value;
            },

            _items: null,
            get items() { return this._items; },
            set items(value) {
                if (Array.isArray(value)) {
                    this._items = [];
                    value.forEach((r, i) => {
                        var row = {};
                        if (Array.isArray(r)) {
                            r.forEach(function (c, j) { row[j] = r[j]; })
                        } else {
                            row = r;
                        }
                        this._items.push(row);
                    });
                }
            },

            load(force) {
                if (this.ajax && (force || this.items == null || datatable.serverSide)) {
                    $.ajax(this.ajax);
                }
            }
        },
        _columns: {
            element: null,
            _items: [],
            get items() { return this._items; },
            set items(value) {
                if (Array.isArray(value)) {
                    value.forEach(function (column, index) {
                        if (!column.data) column.data = index;
                        if (!column.type) column.type = 'text';
                        if (!column.show) column.show = true;
                        if (!column.order) column.order = index;

                        if (column.render) {
                            if (typeof column.render === 'string') {
                                var field = column.render;
                                column.render = function (row) { return row[this.data][field]; }
                            }
                        } else {
                            column.render = function (row) {
                                if (row['@' + this.data + '-render']) return row['@' + this.data + '-render'];
                                return row[this.data];
                            }
                        }
                        if (column.sorter) {
                            if (typeof column.sorter === 'string') {
                                var field = column.sorter;
                                column.sorter = function (row) { return row[this.data][field]; }
                            }
                        } else column.sorter = function (row) {
                            if (row['@' + this.data + '-sort']) return row['@' + this.data + '-sort'];
                            return row[this.data];
                        }
                        if (column.searcher) {
                            if (typeof column.searcher === 'string') {
                                var field = column.searcher;
                                column.searcher = function (row) { return row[this.data][field]; }
                            }
                        } else column.searcher = function (row) {
                            if (row['@' + this.data + '-search']) return row['@' + this.data + '-search'];
                            return row[this.data];
                        }
                    });
                    this._items = value;
                }
            },
            render: function () {
                this.items.forEach(function (column, index) {
                    if (!column.name) column.name = index;
                    if (column.sortable == null) {
                        if (column.type == 'control') column.sortable = false;
                        else column.sortable = true;
                    }
                })

                var columns_tr = this.element.querySelector('.sdt-columns');
                var firstRender = !columns_tr;
                if (firstRender) {
                    columns_tr = document.createElement('tr');
                    columns_tr.classList.add('sdt-columns')
                    this.element.appendChild(columns_tr);
                } else columns_tr.innerHTML = '';

                this.items.forEach((column) => {
                    column.element = document.createElement('th');
                    if (column.width) column.element.style.width = column.width;
                    if (column.sortable) column.element.classList.add('sortable');
                    if (datatable.orderby == column) column.element.classList.add(datatable.ascending ? 'asc' : 'desc');
                    var th_a = document.createElement('a');
                    var a_text = document.createElement('span');
                    a_text.classList.add('sdt-text');
                    a_text.innerHTML = column.text;
                    th_a.appendChild(a_text);
                    var a_icon = document.createElement('span');
                    a_icon.classList.add('sdt-icon');
                    th_a.appendChild(a_icon);

                    if (column.sortable) {
                        th_a.addEventListener('click', () => {
                            if (datatable.orderby == column) datatable.ascending = (datatable.ascending ? false : true);
                            else datatable.ascending = true;
                            datatable.orderby = column;
                            this.render();
                            datatable.rows.render();
                        });
                    }
                    column.element.appendChild(th_a);
                    columns_tr.appendChild(column.element);
                });
                if (firstRender) {
                    var filters_tr = document.createElement('tr');
                    filters_tr.classList.add('sdt-filters')
                    this.items.forEach((column, index) => {
                        column.filter = column.filter || {};
                        var column_td = document.createElement('td');
                        column.filter.element = document.createElement('div');

                        var opt = document.createElement('div');
                        opt.classList.add('opt')
                        var opt_a = document.createElement('a');
                        opt_a.innerHTML = '&#9776;'
                        opt.appendChild(opt_a);
                        var opt_box = document.createElement('ul');
                        //=========== Filters ===========//
                        var opt_smaller = document.createElement('li');
                        opt_smaller.classList.add('smaller');
                        var opt_smaller_a = document.createElement('a');
                        opt_smaller_a.innerHTML = '<';
                        opt_smaller_a.title = 'Smaller';
                        opt_smaller_a.addEventListener('click', () => { column.filter.mode = 'smaller'; opt_a.className = 'smaller'; datatable.rows.render(); });
                        opt_smaller.appendChild(opt_smaller_a);
                        //==
                        var opt_equal = document.createElement('li');
                        opt_equal.classList.add('equal');
                        var opt_equal_a = document.createElement('a');
                        opt_equal_a.innerHTML = '=';
                        opt_equal_a.title = 'Equal';
                        opt_equal_a.addEventListener('click', () => { column.filter.mode = 'equal'; opt_a.className = 'equal'; datatable.rows.render(); });
                        opt_equal.appendChild(opt_equal_a);
                        //==
                        var opt_notequal = document.createElement('li');
                        opt_notequal.classList.add('notequal');
                        var opt_notequal_a = document.createElement('a');
                        opt_notequal_a.innerHTML = '!=';
                        opt_notequal_a.title = 'Not Equal';
                        opt_notequal_a.addEventListener('click', () => { column.filter.mode = 'notequal'; opt_a.className = 'notequal'; datatable.rows.render(); });
                        opt_notequal.appendChild(opt_notequal_a);
                        //==
                        var opt_greater = document.createElement('li');
                        opt_greater.classList.add('greater');
                        var opt_greater_a = document.createElement('a');
                        opt_greater_a.innerHTML = '>';
                        opt_greater_a.title = 'Greater';
                        opt_greater_a.addEventListener('click', () => { column.filter.mode = 'greater'; opt_a.className = 'greater'; datatable.rows.render(); });
                        opt_greater.appendChild(opt_greater_a);
                        //==
                        var opt_contain = document.createElement('li');
                        opt_contain.classList.add('contain');
                        var opt_contain_a = document.createElement('a');
                        opt_contain_a.innerHTML = 'Contain (≃)';
                        opt_contain_a.addEventListener('click', () => { column.filter.mode = 'contain'; opt_a.className = 'contain'; datatable.rows.render(); });
                        opt_contain.appendChild(opt_contain_a);
                        //==
                        var opt_regex = document.createElement('li');
                        opt_regex.classList.add('regex');
                        var opt_regex_a = document.createElement('a');
                        opt_regex_a.innerHTML = 'Regex (®)';
                        opt_regex_a.addEventListener('click', () => { column.filter.mode = 'regex'; opt_a.className = 'regex'; datatable.rows.render(); });
                        opt_regex.appendChild(opt_regex_a);
                        //==
                        var opt_between = document.createElement('li');
                        opt_between.classList.add('between');
                        var opt_between_a = document.createElement('a');
                        opt_between_a.innerHTML = 'Between (~)';
                        opt_between_a.addEventListener('click', () => { column.filter.mode = 'between'; opt_a.className = 'between'; datatable.rows.render(); });
                        opt_between.appendChild(opt_between_a);
                        //==

                        //===============================//
                        switch (column.type) {
                            case 'select':
                                column.filter.input = document.createElement('select');
                                column.filter.element.addEventListener('change', () => { datatable.rows.render(); });
                                column.filter.getValue = function () { return column.filter.input.value; }
                                const valueMember = column.filter.valueMember || 'key';
                                const displayMember = column.filter.displayMember || 'value';
                                var option = document.createElement('option');
                                option.value = '';
                                option.innerHTML = '';
                                column.filter.input.appendChild(option);
                                if (!Array.isArray(column.filter.items)) {
                                    column.filter.items = [];
                                    for (var i = 0; i < datatable.data.items.length; i++) {
                                        const item = datatable.data.items[i];
                                        if (!column.filter.items.find(x => column.searcher(x) == column.searcher(item))) {
                                            column.filter.items.push({ key: column.searcher(item), value: column.render(item) });
                                        }
                                    }
                                }
                                column.filter.items.forEach(function (item, index) {
                                    var option = document.createElement('option');
                                    if (typeof item === "object") {
                                        option.value = item[valueMember];
                                        option.innerHTML = item[displayMember];
                                    } else {
                                        option.value = item;
                                        option.innerHTML = item;
                                    }
                                    column.filter.input.appendChild(option);
                                });
                                //==== Filters ====//
                                opt_box.appendChild(opt_notequal);
                                opt_box.appendChild(opt_equal);
                                //================//
                                break;
                            case 'text':
                                column.filter.input = document.createElement('input');
                                column.filter.element.addEventListener('keyup', () => { datatable.rows.render(); });
                                column.filter.getValue = function () { return column.filter.input.value; }
                                //==== Filters ====//
                                opt_box.appendChild(opt_smaller);
                                opt_box.appendChild(opt_notequal);
                                opt_box.appendChild(opt_equal);
                                opt_box.appendChild(opt_greater);
                                opt_box.appendChild(opt_contain);
                                opt_box.appendChild(opt_regex);
                                opt_box.appendChild(opt_between);
                            //================//
                        }
                        if (column.filter.input) {
                            column.filter.input.classList.add('input');
                            column.filter.element.appendChild(column.filter.input);
                            if (opt_box.children.length > 0) {
                                opt.appendChild(opt_box);
                                column.filter.element.appendChild(opt);
                            }
                            column_td.appendChild(column.filter.element);
                        }
                        filters_tr.appendChild(column_td);
                    });
                    this.element.appendChild(filters_tr);
                }
            }
        },
        _rows: {
            _filteredItemsCount: 0,
            _showedItemsCount: 0,

            get showedItemsCount() { return this._showedItemsCount; },
            get filteredItemsCount() { return this._filteredItemsCount; },

            element: null,
            items: [],

            render: function () {
                datatable.data.load();

                var tbodies = this.element.querySelectorAll('tbody');
                for (var i = 0; i < tbodies.length; i++) tbodies[i].remove();

                if (!Array.isArray(datatable.data.items)) return;
                //============== Find Items ==============//
                this.items = datatable.data.items.filter(x => true);
                datatable.columns.items.forEach((column, index) => {
                    var mode = column.filter.mode || 'contain';
                    var value = column.filter.getValue ? column.filter.getValue() : null;
                    if (value) {
                        var func;
                        if (mode == 'smaller') func = (x) => column.searcher(x) < value;
                        else if (mode == 'equal') func = (x) => column.searcher(x) == value;
                        else if (mode == 'notequal') func = (x) => column.searcher(x) != value;
                        else if (mode == 'greater') func = (x) => column.searcher(x) > value;
                        else if (mode == 'regex') {
                            value = value.toString();
                            func = (x) => {
                                try {
                                    return column.searcher(x).toString().match(value);
                                } catch (e) { return true; }
                            }
                        }
                        else if (mode == 'between') {
                            var split = value.toString().split('~');
                            if (split.length == 2) {
                                var value1 = split[0].trim();
                                var value2 = split[1].trim();
                                func = (x) => { var searchValue = column.searcher(x); return searchValue >= value1 && searchValue <= value2; };
                            } else func = (x) => true;
                        }
                        else {
                            value = value.toString().toLowerCase();
                            func = (x) => column.searcher(x).toString().toLowerCase().indexOf(value) > -1;
                        }
                        this.items = this.items.filter(func);
                    }
                });
                this._filteredItemsCount = this.items.length;

                if (datatable.orderby) {
                    this.items.sort((n1, n2) => {
                        n1 = datatable.orderby.sorter(n1);
                        n2 = datatable.orderby.sorter(n2);
                        if (datatable.ascending) {
                            if (n1 > n2) return 1;
                            if (n1 < n2) return -1;
                        } else {
                            if (n1 < n2) return 1;
                            if (n1 > n2) return -1;
                        }
                        return 0;
                    });
                }

                this.items = this.items.slice(datatable.rowStartIndex, datatable.rowEndIndex);
                this._showedItemsCount = this.items.length;
                //========================================//

                //============== Show Items ==============//
                for (var i = 0; i < this.items.length; i++) {
                    const row = this.items[i];
                    var tbody = document.createElement('tbody');
                    var tr = document.createElement('tr');
                    for (var j = 0; j < datatable.columns.items.length; j++) {
                        const column = datatable.columns.items[j];
                        var td = document.createElement('td');
                        td.innerHTML = column.render(row);
                        tr.appendChild(td);
                    }
                    tbody.appendChild(tr);
                    this.element.appendChild(tbody);
                }
                //=======================================//

                datatable.pagination.render();
                datatable.info.render();

                datatable.onrowsRendered.emit();
            }
        },
        _pagination: {
            element: null,
            format: '{page_t} {page} {of} {total}',
            render: function () {
                this.element.innerHTML = '';
                var ul = document.createElement('ul');
                //==== First Begin ====//
                var first_li = document.createElement('li');
                var first_li_a = document.createElement('a');
                first_li_a.classList.add('sdt-first-page');
                first_li_a.innerHTML = '<<';
                first_li_a.addEventListener('click', () => { datatable.pageFirst(); });
                first_li.appendChild(first_li_a);
                ul.appendChild(first_li);
                //==== First End ====//
                //==== Prev Begin ====//
                var prev_li = document.createElement('li');
                var prev_li_a = document.createElement('a');
                prev_li_a.classList.add('sdt-prev-page');
                prev_li_a.innerHTML = '<';
                prev_li_a.addEventListener('click', () => { datatable.pagePrev(); });
                prev_li.appendChild(prev_li_a);
                ul.appendChild(prev_li);
                //==== Prev End ====//
                //==== Next Begin ====//
                var next_li = document.createElement('li');
                var next_li_a = document.createElement('a');
                next_li_a.classList.add('sdt-next-page');
                next_li_a.innerHTML = '>';
                next_li_a.addEventListener('click', () => { datatable.pageNext(); });
                next_li.appendChild(next_li_a);
                ul.appendChild(next_li);
                //==== Next End ====//
                //==== Last Begin ====//
                var last_li = document.createElement('li');
                var last_li_a = document.createElement('a');
                last_li_a.classList.add('sdt-last-page');
                last_li_a.innerHTML = '>>';
                last_li_a.addEventListener('click', () => { datatable.pageLast(); });
                last_li.appendChild(last_li_a);
                ul.appendChild(last_li);
                //==== Last End ====//
                this.element.appendChild(ul);

                var span = document.createElement('span');
                span.innerHTML = `صفحۀ ${datatable.page} از ${datatable.totalPage}`;
                this.element.appendChild(span);
            }
        },
        _info: {
            element: null,
            render: function () {
                this.element.innerHTML = '';
                var span = document.createElement('span');
                var start = datatable.rowStartIndex == 0 && datatable.count > 0 ? datatable.rowStartIndex + 1 : datatable.rowStartIndex;
                var end = datatable.rowEndIndex;
                span.innerHTML = `نمایش ${start} ~ ${end} از ${datatable.count} ردیف`
                this.element.appendChild(span);
            }
        },
        //==============================================
        width: config.width || '100%',
        height: config.height || 'auto',
        paging: true,
        serverSide: false,
        selectRow: false,
        selectColumn: false,
        selectCell: false,
        //============================================= API
        //=== Properties
        get count() { if (this.serverSide) return this._count; return this.rows.filteredItemsCount; },
        set count(value) { this._count = value; },
        get page() {
            if (this._page < 1) this._page = 1;
            else if (this._page > this.totalPage) this._page = this.totalPage;
            return this._page;
        },
        set page(value) {
            if (value < 1) this._page = 1;
            else if (value > this.totalPage) this._page = this.totalPage;
            else this._page = value;
            datatable.rows.render();
        },
        get totalPage() { return Math.ceil(this.count / this.limit) },
        get rowStartIndex() { return datatable.limit * (datatable.page > 0 ? datatable.page - 1 : 0); },
        get rowEndIndex() { return this.rowStartIndex + datatable.limit },
        get limit() { return this._limit; },
        set limit(value) {
            if (this.limitation.items.find(x => x == value)) this._limit = value;
            else this._limit = this.limitation.items[0];
            datatable.rows.render();
        },
        get buttons() { return this._buttons; },
        set buttons(value) {
            if (!value) return;
            if (Array.isArray(value)) this._buttons.items = value;
            else {
                this._buttons.items = value.items;
            }
        },
        get limitation() { return this._limitation; },
        set limitation(value) {
            if (!value) return;
            if (Array.isArray(value)) this._limitation.items = value;
            else this._limitation = value;
        },
        get data() { return this._data; },
        set data(value) {
            if (!value) return;
            if (Array.isArray(value)) this._data.items = value;
            else {
                if (config.data.ajax) datatable.data.ajax = config.data.ajax;

            }
        },
        get columns() { return this._columns; },
        set columns(value) {
            if (!value) return;
            if (Array.isArray(value)) this._columns.items = value;
            else {
                this._columns.items = value.items;
            }
        },
        get rows() { return this._rows; },
        set rows(value) {
            if (!value) return;
            if (Array.isArray(value)) this._rows.items = value;
            else this._rows = value;
        },
        get pagination() { return this._pagination; },
        set pagination(value) {
            if (!value) return;
            if (Array.isArray(value)) this._pagination.items = value;
            else {
                this._pagination.pageNumberEnable = value.pageNumberEnable ? true : false;
                this._pagination.nextpageNumberCounts = value.nextpageNumberCounts ? true : false;
                this._pagination.previouspageNumberCounts = value.previouspageNumberCounts ? true : false;
                this._pagination.firstPageEnable = value.firstPageEnable ? true : false;
                this._pagination.lastPageEnable = value.lastPageEnable ? true : false;
                this._pagination.nextPageEnable = value.nextPageEnable ? true : false;
                this._pagination.previousPageEnable = value.previousPageEnable ? true : false;
            }
        },
        get info() { return this._info; },
        set info(value) {
            if (!value) return;
            if (Array.isArray(value)) this._info.items = value;
            else this._info = value;
        },
        //=== Methods
        pageFirst: function () { this.page = 1; },
        pagePrev: function () { this.page--; },
        pageNext: function () { this.page++; },
        pageLast: function () { this.page = 1000; },
        render: function () {
            this.buttons.render();
            this.limitation.render();
            this.columns.render();
            this.rows.render();
            this.pagination.render();
            this.info.render();
        },
        load() {
            this.data.load(true);
        },
        initialize() {
            this.element.classList.add('sdt');
            this.element.classList.add(this._rtl ? 'rtl' : 'ltr');
            if (this.width) this.element.style.width = this.width;
            if (this.height) this.element.style.height = this.height;
            //===================== Top Begin ======================//
            var top_div = document.createElement('div');
            top_div.classList.add('sdt-top');
            //===================== Buttons Begin ======================//
            this.buttons.element = document.createElement("div");
            this.buttons.element.classList.add('sdt-buttons');
            top_div.append(this.buttons.element);
            //===================== Buttons End ======================//
            //===================== Limitation Begin ======================//
            this.limitation.element = document.createElement('div');
            this.limitation.element.classList.add('sdt-limitation');
            top_div.appendChild(this.limitation.element);
            //===================== Limitation End ======================//
            this.element.append(top_div);
            //===================== Top End ======================//
            //===================== Data Begin ======================//
            this.data.element = this.rows.element = document.createElement("table");
            this.data.element.classList.add('sdt-data');
            this.columns.element = document.createElement('thead');
            this.data.element.appendChild(this.columns.element);
            this.element.append(this.data.element);
            //===================== Data End ======================//
            //===================== Bottom Begin ======================//
            var bottom_div = document.createElement("div");
            bottom_div.classList.add('sdt-bottom');
            //===================== Pagination Begin ======================//
            this.pagination.element = document.createElement('div');
            this.pagination.element.classList.add('sdt-pagination');
            bottom_div.appendChild(this.pagination.element);
            //===================== Pagination End ======================//
            //===================== Info Begin ======================//
            this.info.element = document.createElement('div');
            this.info.element.classList.add('sdt-info');
            bottom_div.appendChild(this.info.element);
            //===================== Info End ======================//
            this.element.appendChild(bottom_div);
            //===================== Bottom End ======================//
            this.render();
        },
        //============================================= Language
        language: {
            processing: "Traitement en cours...",
            search: "Rechercher&nbsp;:",
            lengthMenu: "Afficher _MENU_ &eacute;l&eacute;ments",
            info: "Affichage de l'&eacute;lement _START_ &agrave; _END_ sur _TOTAL_ &eacute;l&eacute;ments",
            infoEmpty: "Affichage de l'&eacute;lement 0 &agrave; 0 sur 0 &eacute;l&eacute;ments",
            infoFiltered: "(filtr&eacute; de _MAX_ &eacute;l&eacute;ments au total)",
            infoPostFix: "",
            loadingRecords: "Chargement en cours...",
            zeroRecords: "Aucun &eacute;l&eacute;ment &agrave; afficher",
            emptyTable: "Aucune donnée disponible dans le tableau",
            paginate: {
                first: "Premier",
                previous: "Pr&eacute;c&eacute;dent",
                next: "Suivant",
                last: "Dernier"
            },
            aria: {
                sortAscending: ": activer pour trier la colonne par ordre croissant",
                sortDescending: ": activer pour trier la colonne par ordre décroissant"
            }
        },
        //============================================= Events
        onrender: new Observable(),
        ondataLoading: new Observable(),
        ondataLoaded: new Observable(),
        onpageChange: new Observable(),
        onlimitChange: new Observable(),
        onrowsRendered: new Observable(),
        onrowSelectedChange: new Observable(),
    };
    datatable.buttons = config.buttons;
    datatable.columns = config.columns;
    datatable.data = config.data;
    datatable.rows = config.rows;
    datatable.pagination = config.pagination;
    datatable.limitation = config.limitation;

    if (config.onrowsRendered) datatable.onrowsRendered.subscribe(config.onrowsRendered);

    //===================== Load Layout Begin ======================//
    if (element.tagName == "TABLE") {
        if (!datatable.columns.items) {
            datatable.columns.items = [];
            var thead = element.querySelector('thead');
            if (thead) {
                thead.find('td, th').each(function (colIndex, col) {
                    datatable.columns.items.push({
                        text: col.innerHTML,
                    })
                });
            }
        }
        if (!datatable.data.items) {
            datatable.data.items = []
            var tbody = element.querySelector('tbody');
            if (tbody) {
                tbody.find('tr').each(function (rowIndex, row) {
                    datatable.data.items.push([]);
                    $('td', tr).each(function (colIndex, col) {
                        datatable.data.items[rowIndex].push(col.innerHTML);
                    });
                });
            }
        }
        var newElement = document.createElement('div');
        element.parentNode.insertBefore(newElement, element);
        var temp = element;
        element = newElement;
        temp.remove();
    }

    datatable.initialize();
    return datatable;
};
