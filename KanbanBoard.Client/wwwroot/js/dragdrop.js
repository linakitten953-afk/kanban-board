window.dragDropHelper = {
    init: function (dotNetHelper) {
        document.addEventListener('dragstart', function (e) {
            if (e.target.classList.contains('kanban-card')) {
                e.target.classList.add('dragging');
                e.dataTransfer.setData('text/plain', e.target.dataset.cardId);
            }
        });

        document.addEventListener('dragend', function (e) {
            if (e.target.classList.contains('kanban-card')) {
                e.target.classList.remove('dragging');
            }
        });

        document.addEventListener('dragover', function (e) {
            if (e.target.closest('.kanban-column-body')) {
                e.preventDefault();
            }
        });

        document.addEventListener('drop', function (e) {
            const columnBody = e.target.closest('.kanban-column-body');
            if (columnBody) {
                e.preventDefault();
                const cardId = parseInt(e.dataTransfer.getData('text/plain'));
                const columnId = parseInt(columnBody.dataset.columnId);
                if (cardId && columnId) {
                    dotNetHelper.invokeMethodAsync('OnCardDropped', cardId, columnId);
                }
            }
        });
    }
};