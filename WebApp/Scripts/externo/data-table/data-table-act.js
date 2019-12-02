(function ($) {
    "use strict";

    $(document).ready(function () {
        $('#data-table-nf-anp').DataTable({
            lengthChange: false,
            paging: false,
            searching: false,
            info: false,
            language: {
                "emptyTable": "Sem dados"
            }
        });
        $('#data-table-cupom').DataTable({
            lengthChange: false,
            paging: false,
            searching: false,
            info: false,
            language: {
                "emptyTable": "Sem dados"
            }
        });
        $('#data-table-outros').DataTable({
            lengthChange: false,
            paging: false,
            searching: false,
            info: false,
            language: {
                "emptyTable": "Sem dados"
            }
        });
    });

})(jQuery); 