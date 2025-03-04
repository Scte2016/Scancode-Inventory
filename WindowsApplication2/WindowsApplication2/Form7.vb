Imports MySql.Data.MySqlClient
Public Class Form7
    Dim serverstring As String = "Server=192.168.1.32;UserId='scancode';Password='admin101';Database=scancode;Convert Zero Datetime=True"
    Dim SQLConnection As MySqlConnection = New MySqlConnection
    Dim cmd As MySqlCommand = New MySqlCommand
    Dim rer As MySqlDataReader
    Dim formattedDate As String = Date.Today.ToString("yyyy/MM/dd")
    Dim ty As String
    Public panga As String
    Public typ As String
    Public Sub loa()
        Try
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT i.code as 'CODE', i.name as 'NAME' , i.descri as 'DESCRIPTION', i.size as 'SIZE',i.typ as 'Type', i.quan as 'QUANTITY',i.dat as 'Last Update',i.unit as 'UNIT', i.price as 'PRICE' FROM items i where quan=0 and unit <> 'Piece';", serverstring)
            adapter.Fill(table)
            DataGridView1.DataSource = table
            DataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            SQLConnection.Close()
            If typ = "User" Then
                DataGridView1.Columns("PRICE").Visible = False
            Else
                DataGridView1.Columns("PRICE").Visible = True

            End If
          
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Form7_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(500, 100)
        SQLConnection.ConnectionString = serverstring
        loa()

    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            Dim row As DataGridViewRow
            row = Me.DataGridView1.Rows(e.RowIndex)
            Form1.TextBox1.Text = row.Cells("CODE").Value
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            Dim row As DataGridViewRow
            row = Me.DataGridView1.Rows(e.RowIndex)
            Form1.TextBox1.Text = row.Cells("CODE").Value
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class