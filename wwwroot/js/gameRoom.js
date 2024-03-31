var tableRoundsArray = [];
var isInitial = true;
var url = "Game/GetQuestionsTable";
var playerScore = 0;
var connection = new signalR.HubConnectionBuilder().withUrl("/onlineUsersHub").build();

connection.on("UpdateOnlineUsers", function (count) {
    document.getElementById("onlineUsersCount").innerText = count;
});

connection.start();
$(document).ready(function () {
        FetchGameData();
});

function FetchGameData() {
    $.ajax({
        url: "Game/GetQuestionsTable",
        type: "POST",
        success: function (response) {
            if (response != null && response != undefined) {
                tableRoundsArray = response;
                if (tableRoundsArray.length > 0) {
                    initializeDataTable(tableRoundsArray);
                    setInterval(RefreshTable, 5000);
                } else {
                    //There were no other active players and no expressions were generated, 
                    //so try to load the game table one interval/game round lenght later
                    setTimeout(function () {
                        FetchGameData();
                    }, 5000);
                }
            }
        }
    });
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
            alert("We are experiencing difficulties at the moment, please try again later");
        }
    });
}