function DrawTable(response) {
    return response.map(function (item) {
        return {
            "roundNumber": item["roundNumber"],
            "expression": item["expression"],
            "yourAnswer": item["yourAnswer"],
            "result": item["result"]
        };
    });
}

function RefreshTable() {
    $.ajax({
        url: "Game/GetQuestionsTableAfterRound?roundNumber=" + tableRoundsArray.length,
        type: "POST",
        success: function (response) {
            if (response != null && response != undefined) {
                response.forEach(function (newRound) {
                    tableRoundsArray.push(newRound);
                });
            }
        }
    });

    $('#questionsTable').DataTable().clear();
    $('#questionsTable').DataTable().rows.add(tableRoundsArray);
    $('#questionsTable').DataTable().draw();
    setTimeout(function () {
        if (tableRoundsArray.length > 1) {
            AddRowForAnswering(tableRoundsArray[tableRoundsArray.length - 1]);
        }
        if (tableRoundsArray.length > 2) {
            RemoveOldButtons(tableRoundsArray[tableRoundsArray.length - 3]);
        }
    }, 400);
}
