(function ($) {
    "use strict";

    $(document).ready(function () {
        $('#data-table-nf-anp').DataTable({
            lengthChange: false,
            language: {
                "emptyTable": "Sem dados dispon�veis"
            }
        });
        $('#data-table-cupom').DataTable({
            lengthChange: false,
            language: {
                "emptyTable": "Sem dados dispon�veis"
            }
        });
        $('#data-table-outros').DataTable({
            lengthChange: false,
            language: {
                "emptyTable": "Sem dados dispon�veis"
            }
        });
    });

})(jQuery); 