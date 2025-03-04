Imports MySql.Data.MySqlClient
Public Class Form6
    Dim serverstring As String = "Server=192.168.1.32;UserId='scancode';Password='admin101';Database=scancode"
    'Dim serverstring As String = "Server=localhost;UserId=root;Password='admin101';Database=scancode"
    Dim SQLConnection As MySqlConnection = New MySqlConnection
    Dim cmd As MySqlCommand = New MySqlCommand
    Dim rer As MySqlDataReader
    Dim formattedDate As String = Date.Today.ToString("yyyy/MM/dd")
    Public Sub loa()

        SQLConnection.Open()
        Dim table As New DataTable()
        Dim adapter As New MySqlDataAdapter("SELECT code as 'Code', quan as 'Quantity', dr as'Delivery Reciept', price as 'Price'  FROM hist where si='Partial Delivery';", serverstring)
        adapter.Fill(table)
        DataGridView1.DataSource = table
        DataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
        SQLConnection.Close()
      
    End Sub
    Private Sub Form6_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SQLConnection.ConnectionString = serverstring
        loa()
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim row As DataGridViewRow
        row = Me.DataGridView1.Rows(e.RowIndex)
        Form1.Label44.Text = row.Cells("Delivery Reciept").Value
        Form1.DataGridView2.Rows.Add(row.Cells("Code").Value, "Partial from Delivery Reciept:" & row.Cells("Delivery Reciept").Value, row.Cells("Quantity").Value, " ", " ", row.Cells("Price").Value, " ")
        Me.Close()
    End Sub
End Class