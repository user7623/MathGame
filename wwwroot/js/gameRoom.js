var roundCounter = 0;

$(document).ready(function () {
    FetchGameData();
    setInterval(InitializeNewRoundTimer, 5000);
    //StartRoundTimer
    //GetLastUnansweredRound
    //BuildRowWithButtonsFromUnansweredRound
});
//WORKFLOW
//You enter, the game up to now gets loaded - done
//timer get initialized
//newest round gets loaded with buttons for answering

function InitializeNewRoundTimer() {
    console.log("initialize");
    GetResultFromCurrentRound();

    //get the newest round
    const url = "https://localhost:44386/Game/GetNewestRound?roomName=" + "test room";//${ roomName };
    $.ajax({
        url: url,
        method: 'GET',
        success: function (data) {
            AddRowForAnswering(data);
        },
        error: function (xhr, status, error) {
            console.error('There was a problem with the AJAX request:', error);
        }
    });
}

function GetResultFromCurrentRound() {
    console.log("GetResultFromCurrentRound");
    debugger;
    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() + roundCounter - 1;
    var newCellValue = 'new value';//TODO
    table.cell(lastRowIndex, -1).data(newCellValue);
    roundCounter++;
}

function FetchGameData() {
    $('#questionsTable').DataTable({
        ajax: {
            url: "Game/GetQuestionsTable",
            type: "POST",
            dataSrc: function (response) {
                debugger;
                if (response != null && response != undefined) {
                    return response.map(function (item) {
                        return {
                            "roundNumber": item["roundNumber"],
                            "expression": item["expression"],
                            "yourAnswer": item["yourAnswer"],
                            "result": item["v"]
                        };
                    });
                } else {
                    //TODO : custom alert
                    alert("We are experiencing difficulties at the moment, please try again later");
                }
            }
        },
        serverSide: true,
        paginate: false,
        filter: false,
        info: false,
        columns: [
            { data: "roundNumber", name: "roundNumber" },
            { data: "expression", name: "expression" },
            { data: "yourAnswer", name: "yourAnswer" },
            { data: "result", name: "result" },
        ],
        initComplete: function () {
            $("#loadingComponent").hide();
            InitializeNewRoundTimer();
        },
        columnDefs: [
            {
                targets: '_all', // Apply to all columns
                defaultContent: '' // Default content for missing columns
            }
        ]
    });
}

//function AddRowForAnswering(data) {
//    // Get reference to the DataTable
//    var table = $('#questionsTable').DataTable();

//    // Add new row data to DataTable's internal data array
//    var newRowData = [
//        data.roundNumber,
//        data.expression,
//        '<button class="answer-button" data-round-number="' + data.roundNumber + '">Yes</button>' +
//        '<button class="answer-button" data-round-number="' + data.roundNumber + '">No</button>',
//        ''
//    ];
//    table.row.add(newRowData);

//    //$('.answer-button').on('click', function () {
//    //    var roundNumber = $(this).data('round-number');
//    //    var answer = $(this).text();
//    //    SubmitAnswer(roundNumber, answer);
//    //});
//}

function AddRowForAnswering(data) {
    var rowHTML = `<tr class="even">
                        <td>` + data.roundNumber + `</td>
                        <td>` + data.expression + `</td>
                        <td>
                            <button class="answer-button" onclick="javascript: SubmitAnswer(` + data.roundNumber + `, Yes)">Yes</button>
                            <button class="answer-button" onclick="javascript: SubmitAnswer(` + data.roundNumber + `, No)">No</button>
                        </td>
                        <td></td>
                    </tr>`;

    const tableBody = $('#questionsTable tbody');
    tableBody.append(rowHTML);

        $('.answer-button').on('click', function () {
        var roundNumber = $(this).data('round-number');
        var answer = $(this).text();
        SubmitAnswer(roundNumber, answer);
    });
}

function LoadGameData(dataArray) {
    var data = dataArray;
    console.log(data);
    console.log("LoadGameData");
    $('#questionsTable').DataTable({
        dataSrc: function (data) {
            if (data !== null && data !== undefined) {
                return data.map(function (item) {
                    console.log(item["roundNumber"], item["expression"]);
                    var answered = "";
                    if (item["yourAnswer"] === undefined || item["yourAnswer"] === null) {
                        answered === " Did not participate"
                    }
                    return {
                        "roundNumber": item["roundNumber"],
                        "expression": item["expression"],
                        "yourAnswer": answered,
                        "result": "TODO"
                    };
                });
            }
        },
        serverSide: true,
        paginate: false,
        filter: false,
        info: false,
        columns: [
            { data: "roundNumber", name: "roundNumber" },
            { data: "expression", name: "expression" },
            { data: "yourAnswer", name: "yourAnswer" },
            { data: "result", name: "result" },
        ],
        columnDefs: [
            {
                targets: '_all',
                defaultContent: ''
            }
        ]
    });
}

function SubmitAnswer(roundNumber, answer) {
    //TODO : read values from input fileds
    //set answered cell data
    //submit answer

    SetYourAnswerCellText(answer);

    //var data = {
    //    Answer: answer,
    //    RoundNumber: roundNumber,
    //    Timestamp: Date.now(),
    //};
    //debugger;
    //$.ajax({
    //    type: "POST",
    //    url: "Game/SubmitAnswer",
    //    contentType: "application/json",
    //    data: JSON.stringify(data),
    //    success: function (data, status) {
    //        alert("Data: " + data + "\nStatus: " + status);
    //    },
    //    error: function (xhr, status, error) {
    //        alert("Error: " + error);
    //    }
    //});
}

function SetYourAnswerCellText(answer) {
    debugger;
    //get the result from current round
    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() + roundCounter - 2;
    var newCellValue = answer;
    table.cell(lastRowIndex, -1).data(newCellValue);
}