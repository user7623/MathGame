var tableRoundsArray = [];
var isInitial = true;
var url = "Game/GetQuestionsTable";
var playerScore = 0;
$(document).ready(function () {
    //initializeDataTable(tableRoundsArray);
    FetchGameData();
    setInterval(RefreshTable, 10000);
    //StartRoundTimer
    //GetLastUnansweredRound
    //BuildRowWithButtonsFromUnansweredRound
});

function FetchGameData() {
    $.ajax({
        url: "Game/GetQuestionsTable",
        type: "POST",
        success: function (response) {
            if (response != null && response != undefined) {
                tableRoundsArray = response;
            }
        }
    });

    initializeDataTable(tableRoundsArray);
    setTimeout(function () {
        AddRowForAnswering(tableRoundsArray[tableRoundsArray.length - 1]);
    }, 400);
}

// Function to initialize DataTable
function initializeDataTable(data) {
    table = $('#questionsTable').DataTable({
        data: data,
        columns: [
            { data: "roundNumber", name: "roundNumber" },
            { data: "expression", name: "expression" },
            { data: "yourAnswer", name: "yourAnswer" },
            { data: "result", name: "result" },
        ],
        initComplete: function () {
            $("#loadingComponent").hide();
        },
        paginate: false,
        filter: false,
        info: false,
        columnDefs: [
            {
                targets: '_all',
                defaultContent: ''
            }
        ]
    });
}

// Function to fetch new data from the server
function fetchDataFromServer(roundNumber) {
    $.ajax({
        url: "Game/GetQuestionsTableAfterRound?roundNumber=" + roundNumber,
        type: "POST",
        success: function (response) {
            if (response != null && response != undefined) {
                table.clear().rows.add(response).draw();
            } else {
                alert("We are experiencing difficulties at the moment, please try again later");
            }
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error("Error:", error);
            alert("We are experiencing difficulties at the moment, please try again later");
        }
    });
}

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
                //tuka da se dopolni tableArray i da se redraw na tabelata
            }
        }
    });

    $('#questionsTable').DataTable().clear();
    $('#questionsTable').DataTable().rows.add(tableRoundsArray);
    $('#questionsTable').DataTable().draw();
    setTimeout(function () {
        AddRowForAnswering(tableRoundsArray[tableRoundsArray.length - 1]);
        RemoveOldButtons(tableRoundsArray[tableRoundsArray.length - 3]);
    }, 400);
}

function AddRowForAnswering(data) {
    var roundNumber = (data.roundNumber - 1).toString();
    var buttonsForAnsweringHtml = ` <button class="answer-button" onclick="javascript: SubmitAnswer(` + roundNumber + `, 'Yes')">Yes</button>
                                    <button class="answer-button" onclick="javascript: SubmitAnswer(` + roundNumber + `, 'No')">No</button>`;

    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() - 1;
    table.cell(lastRowIndex, -2).data(buttonsForAnsweringHtml);
}

function RemoveOldButtons(data) {
    if (data.result === "") {
        var table = $('#questionsTable').DataTable();
        var lastRowIndex = table.rows().count() - 2;
        table.cell(lastRowIndex, -2).data("MISSED");
        table.cell(lastRowIndex, -1).data("FAILED");
    }
}

function AddAnswerToTableArray(roundNumber, answer, response) {
    var newRowData = tableRoundsArray[roundNumber - 1];
    if (response === 1) {
        newRowData.result = "CORRECT";
        newRowData.answer = answer;

        playerScore++;

        $("#playerScore").text(playerScore);
    }
    if (response === 0) {
        newRowData.result = "MISSED";
        newRowData.answer = answer;
    }
    if (response === -1) {
        newRowData.result = "FAILED";
        newRowData.answer = answer;
    }
    tableRoundsArray[roundNumber - 1] = newRowData;
    console.log(tableRoundsArray);
}

function SubmitAnswer(roundNumber, answer) {
    SetYourAnswerCellText(answer);
    var data = {
        Answer: answer,
        RoundNumber: roundNumber,
        Timestamp: Date.now(),
        RoomName: "test room",
    };
    $.ajax({
        type: "POST",
        url: "Game/SubmitAnswer",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response !== null && response !== undefined) {
                AddAnswerToTableArray(roundNumber, answer, response);
            } else {
                alert("We are experiencing difficulties at the moment, please try again later");
            }
        },
        error: function (xhr, status, error) {
            alert("We are experiencing difficulties at the moment, please try again later");
        }
    });
}

function SetYourAnswerCellText(answer) {
    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() - 1;
    var newCellValue = answer;
    table.cell(lastRowIndex, -2).data(newCellValue);
}