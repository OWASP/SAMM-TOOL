/**
 * A small plugin for getting the CSV of a categorized chart
 */
(function (Highcharts) {

    var ConvertRawToVals = function (raw) {
        switch (raw) {
            case 0:
                return "0";
                break;
            case 1:
                return "0.5";
                break;
            case 2:
                return "1";
                break;
            case 3:
                return "1.5";
                break;
            case 4:
                return "2";
                break;
            case 5:
                return "2.5";
                break;
            case 6:
                return "3";
                break;
            default:
                return raw;
                break;
        };
    };
    var each = Highcharts.each;
    Highcharts.Chart.prototype.getCSV = function () {
        var columns = [],
            line,
            tempLine,
            csv = "",
            row,
            col,
            options = (this.options.exporting || {}).csv || {},

            // Options
            dateFormat = options.dateFormat || '%Y-%m-%d %H:%M:%S',
            itemDelimiter = options.itemDelimiter || ',', // use ';' for direct import to Excel
            lineDelimiter = options.lineDelimeter || '\n';

        each(this.series, function (series) {
            if (series.options.includeInCSVExport !== false) {
                if (series.xAxis) {
                    var xData = series.xData.slice(),
                        xTitle = 'X values';
                    if (series.xAxis.isDatetimeAxis) {
                        xData = Highcharts.map(xData, function (x) {
                            return Highcharts.dateFormat(dateFormat, x)
                        });
                        xTitle = 'DateTime';
                    } else if (series.xAxis.categories) {
                        xData = Highcharts.map(xData, function (x) {
                            return Highcharts.pick(series.xAxis.categories[x], x);
                        });
                        xTitle = 'Category';
                    }
                    columns.push(xData);
                    columns[columns.length - 1].unshift(xTitle);
                }
                columns.push(series.yData.slice());
                columns[columns.length - 1].unshift(series.name);
            }
        });

        // Transform the columns to CSV
        for (row = 0; row < columns[0].length; row++) {
            line = [];
            for (col = 0; col < columns.length; col++) {
                if (col != 2 && col < 4){
                    line.push(ConvertRawToVals(columns[col][row]));
                }
            }
            csv += line.join(itemDelimiter) + lineDelimiter;
        }

        return csv;
    };


}(Highcharts));


