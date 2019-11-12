(function ($) {
    "use strict";

    $(document).ready(function () {
        $('#data-table-nf-anp').DataTable({
            lengthChange: false,
            language: {
                "emptyTable": "Sem dados disponíveis"
            }
        });
        $('#data-table-cupom').DataTable({
            lengthChange: false,
            language: {
                "emptyTable": "Sem dados disponíveis"
            }
        });
        $('#data-table-outros').DataTable({
            lengthChange: false,
            language: {
                "emptyTable": "Sem dados disponíveis"
            }
        });
    });

})(jQuery); 