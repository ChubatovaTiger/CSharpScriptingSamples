
#r "nuget:ClosedXML,0.94.2"

using ClosedXML.Excel;
using System.Linq;
using System.Text;

using (var workbook = new XLWorkbook("Sample.xlsx"))
{
    var ws = workbook.Worksheet(1);
    
    var tableName = GetTableName(ws);
    var columnNames = GetColumnNames(ws);
    var valueRows = GetValueRows(ws);
    var values = new List<IEnumerable<string>>();
    foreach (var row in valueRows) {
        var rowValues = GetValuesFromRow(row);
        values.Add(rowValues);
    }

    var sql = PrepareInsertStatement(tableName, columnNames, values);

    Console.WriteLine(sql);
}

private string PrepareInsertStatement(string tableName, IEnumerable<string> columnNames, IEnumerable<IEnumerable<string>> values) {
    var sb = new StringBuilder();
    var columns = CreateColumnNamesStatement(columnNames);
    sb.AppendLine($@"INSERT INTO {tableName} ({columns}) VALUES");
    
    var separator = ",";
    foreach (var row in values) {
        if (row == values.Last()) {
            separator = ";";
        }

        var rowValues = CreateValuesStatement(row);
        sb.AppendLine($@"({rowValues}){separator}");
    }

    return sb.ToString();
}

private string GetTableName(IXLWorksheet worksheet) {
    if (worksheet.Cell(1,1).TryGetValue<string>(out var tableName)) {
        return tableName;
    }
    else {
        return "table name not found";
    }
}

private IEnumerable<string> GetColumnNames(IXLWorksheet worksheet) {
    return worksheet.Row(2).CellsUsed().Select(x => x.Value.ToString());
}

private string CreateColumnNamesStatement(IEnumerable<string> columnNames) {
    return string.Join(',', columnNames);
}

private IXLRows GetValueRows(IXLWorksheet worksheet) {
    return worksheet.RowsUsed(x => x.RowNumber() > 2);
}

private string CreateValuesStatement(IEnumerable<string> values) {
    return string.Join(',', values);
}

private IEnumerable<string> GetValuesFromRow(IXLRow row) {
    return row.CellsUsed().Select(x => x.Value.ToString());
}