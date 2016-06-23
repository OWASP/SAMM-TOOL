
//Convert the string rating score into an int
var ConvertValsToRaw = function (val) {
    switch (val.toString()) {
        case "0":
            return 0;
            break;
        case "0+":
            return 1;
            break;
        case "1":
            return 2;
            break;
        case "1+":
            return 3;
            break;
        case "2":
            return 4;
            break;
        case "2+":
            return 5;
            break;
        case "3":
            return 6;
            break;
        default:
            return 0;
            break;
    };
};

//Convert a raw int into the string rating
var ConvertRawToVals = function (raw) {
    switch (raw) {
        case 0:
            return "0";
            break;
        case 1:
            return "0+";
            break;
        case 2:
            return "1";
            break;
        case 3:
            return "1+";
            break;
        case 4:
            return "2";
            break;
        case 5:
            return "2+";
            break;
        case 6:
            return "3";
            break;
        default:
            return "0";
            break;
    };
};

//Update the chart with values and redraw it - zing!
var GoChartGo = function (somechart, sometitles, somevals, chartcolors, charttitle) {
    rawvals = {
        smc: ConvertValsToRaw(somevals['smc']),
        smt: ConvertValsToRaw(somevals['smt']),
        pcc: ConvertValsToRaw(somevals['pcc']),
        pct: ConvertValsToRaw(somevals['pct']),
        egc: ConvertValsToRaw(somevals['egc']),
        egt: ConvertValsToRaw(somevals['egt']),
        tac: ConvertValsToRaw(somevals['tac']),
        tat: ConvertValsToRaw(somevals['tat']),
        src: ConvertValsToRaw(somevals['src']),
        srt: ConvertValsToRaw(somevals['srt']),
        sac: ConvertValsToRaw(somevals['sac']),
        sat: ConvertValsToRaw(somevals['sat']),
        drc: ConvertValsToRaw(somevals['drc']),
        drt: ConvertValsToRaw(somevals['drt']),
        crc: ConvertValsToRaw(somevals['crc']),
        crt: ConvertValsToRaw(somevals['crt']),
        stc: ConvertValsToRaw(somevals['stc']),
        stt: ConvertValsToRaw(somevals['stt']),
        vmc: ConvertValsToRaw(somevals['vmc']),
        vmt: ConvertValsToRaw(somevals['vmt']),
        ehc: ConvertValsToRaw(somevals['ehc']),
        eht: ConvertValsToRaw(somevals['eht']),
        oec: ConvertValsToRaw(somevals['oec']),
        oet: ConvertValsToRaw(somevals['oet'])
    };

    /* Chart */
    somechart = new Highcharts.Chart({
        chart: {
            renderTo: 'graphycontainer',
            type: 'bar',
            spacingRight: 30
        },
        credits: {
            enabled: false
        },
        title: {
            text: charttitle
        },
        xAxis: {
            categories: [sometitles['smtitle'],
                sometitles['pctitle'],
                sometitles['egtitle'],
                sometitles['tatitle'],
                sometitles['srtitle'],
                sometitles['satitle'],
                sometitles['drtitle'],
                sometitles['crtitle'],
                sometitles['sttitle'],
                sometitles['vmtitle'],
                sometitles['ehtitle'],
                sometitles['oetitle']
            ]
        },
        yAxis: {
            labels: {
                enabled: false
            },
            gridLineColor: 'white',
            title: null
        },
        legend: {
            enabled: false
        },
        tooltip: {
            formatter: function () {
                var s = '<b>' + this.x + '</b>';
                s += '<br /><b>Target:</b> ' + ConvertRawToVals(this.points[1].y);
                s += '<br /><b>Current:</b> ' + ConvertRawToVals(this.points[0].y);
                return s;
            },
            shared: true
        },
        series: [{
            name: sometitles['current'],
            dataLabels: {
                enabled: true,
                color: "#000000",
                formatter: function () {
                    return this.point.name;
                }
            },
            data: [
                { name: "Current: " + somevals['smc'], color: chartcolors['smcolor'], y: rawvals['smc'] },
                { name: "Current: " + somevals['pcc'], color: chartcolors['pccolor'], y: rawvals['pcc'] },
                { name: "Current: " + somevals['egc'], color: chartcolors['egcolor'], y: rawvals['egc'] },
                { name: "Current: " + somevals['tac'], color: chartcolors['tacolor'], y: rawvals['tac'] },
                { name: "Current: " + somevals['src'], color: chartcolors['srcolor'], y: rawvals['src'] },
                { name: "Current: " + somevals['sac'], color: chartcolors['sacolor'], y: rawvals['sac'] },
                { name: "Current: " + somevals['drc'], color: chartcolors['drcolor'], y: rawvals['drc'] },
                { name: "Current: " + somevals['crc'], color: chartcolors['crcolor'], y: rawvals['crc'] },
                { name: "Current: " + somevals['stc'], color: chartcolors['stcolor'], y: rawvals['stc'] },
                { name: "Current: " + somevals['vmc'], color: chartcolors['vmcolor'], y: rawvals['vmc'] },
                { name: "Current: " + somevals['ehc'], color: chartcolors['ehcolor'], y: rawvals['ehc'] },
                { name: "Current: " + somevals['oec'], color: chartcolors['oecolor'], y: rawvals['oec'] }
            ]
        }, {
            name: sometitles['target'],
            dataLabels: {
                enabled: true,
                color: "#000000",
                formatter: function () {
                    return this.point.name;
                }
            },
            data: [

                { name: "Target: " + somevals['smt'], color: chartcolors['smcolor'], y: rawvals['smt'] },
                { name: "Target: " + somevals['pct'], color: chartcolors['pccolor'], y: rawvals['pct'] },
                { name: "Target: " + somevals['egt'], color: chartcolors['egcolor'], y: rawvals['egt'] },
                { name: "Target: " + somevals['tat'], color: chartcolors['tacolor'], y: rawvals['tat'] },
                { name: "Target: " + somevals['srt'], color: chartcolors['srcolor'], y: rawvals['srt'] },
                { name: "Target: " + somevals['sat'], color: chartcolors['sacolor'], y: rawvals['sat'] },
                { name: "Target: " + somevals['drt'], color: chartcolors['drcolor'], y: rawvals['drt'] },
                { name: "Target: " + somevals['crt'], color: chartcolors['crcolor'], y: rawvals['crt'] },
                { name: "Target: " + somevals['stt'], color: chartcolors['stcolor'], y: rawvals['stt'] },
                { name: "Target: " + somevals['vmt'], color: chartcolors['vmcolor'], y: rawvals['vmt'] },
                { name: "Target: " + somevals['eht'], color: chartcolors['ehcolor'], y: rawvals['eht'] },
                { name: "Target: " + somevals['oet'], color: chartcolors['oecolor'], y: rawvals['oet'] }
            ]
        }, {
            name: sometitles['max'],
            data: [
                { color: 'white', y: 6 }
            ]
        }],

        exporting: {
            buttons: {
                contextButton: {
                    menuItems: [{
                        text: 'Export to CSV',
                        onclick: function () {
                            document.getElementById('ExportCSV').click();
                        }
                    }, {
                        text: 'Export to PDF',
                        onclick: function () {
                            this.exportChart({
                                type: 'application/pdf',
                                filename: this.title.text,
                                sourceWidth: 1200,
                                sourceHeight: 600
                            });
                        }
                    }, {
                        text: 'Export to PNG',
                        onclick: function () {
                            this.exportChart({
                                filename: this.title.text,
                                sourceWidth: 1200,
                                sourceHeight: 600
                            });
                        },
                        separator: false
                    }]
                }
            }
        }

    });

    return somechart;

};

;

