Imports MySql.Data.MySqlClient
Public Class Form2
    Dim qr As New MessagingToolkit.QRCode.Codec.QRCodeEncoder
    Public code As String
    Public siz As String
    Dim serverstring As String = "Server=192.168.1.32;UserId='scancode';Password='admin101';Database=scancode"
    Dim SQLConnection As MySqlConnection = New MySqlConnection
    Dim cmd As MySqlCommand = New MySqlCommand
    Dim rer As MySqlDataReader
    Dim formattedDate As String = Date.Today.ToString("yyyy/MM/dd")
    Dim tot As Integer
    Dim un As String
    Public typ As String
    Dim pn As String
    Dim ps As String
    Dim pd As String
    Public Sub sear()
        Dim wew As String = "SELECT * FROM items i where i.code= '" & code & "' and size= '" & siz & "' ;"
        cmd = New MySqlCommand(wew, SQLConnection)
        SQLConnection.Open()
        rer = cmd.ExecuteReader
        While rer.Read
            Dim cou = rer.GetString("code")
            Dim nam = rer.GetString("name")
            Dim des = rer.GetString("descri")
            Dim siz = rer.GetString("size")
            Dim qua = rer.GetInt32("quan")
            Dim dat = rer.GetString("dat")
            Dim ty = rer.GetString("typ")
            Dim uni = rer.GetString("unit")
            Dim pri = rer.GetDouble("price")
            Label2.Text = "Product Code: " & cou
            Label3.Text = "Product Name: " & nam
            Label4.Text = "Product Descrption: " & des
            Label5.Text = "Product Size: " & siz
            Label6.Text = "Product Quantity: " & qua
            Label8.Text = "Product Type: " & ty
            Label9.Text = "Product Unit: " & uni
            pn = nam
            ps = siz
            pd = des
            If ty = "Machine" Then
                Label5.Text = "Product Serial: " & siz
                Label10.Text = "Serial No."
                TextBox2.Text = siz
                TextBox2.Enabled = False


            Else
                Label10.Text = uni & "/Box"
                TextBox2.Enabled = True
                TextBox2.Text = ""
            End If
            TextBox1.Text = pri
            un = uni

            PictureBox1.Image = qr.Encode(code & " , " & Label3.Text & " , " & Label5.Text)
        End While
        SQLConnection.Close()
    End Sub

    Public Sub che()
        For r = 0 To DataGridView1.RowCount - 1
            tot = tot + DataGridView1.Rows(r).Cells("QUANTITY").Value
            If DataGridView1.Rows(r).Cells("QUANTITY").Value < 0 Then
                DataGridView1.Rows(r).Cells("QUANTITY").Style.ForeColor = Color.Red
            End If
        Next
    End Sub
    Public Sub loa()
        Try
            Dim las As Integer
            tot = 0
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT  h.quan as 'QUANTITY', h.name as 'Prepared By', h.dat as 'Date Updated' ,h.company as 'Company', h.dr as 'Delivery Reciept', h.si as 'Sales Invoice'FROM hist h where code = '" & code & "';", serverstring)
            adapter.Fill(table)
            DataGridView1.DataSource = table
            DataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            SQLConnection.Close()
            For r = 0 To DataGridView1.RowCount - 1
                tot = tot + DataGridView1.Rows(r).Cells("QUANTITY").Value
                If DataGridView1.Rows(r).Cells("QUANTITY").Value < 0 Then
                    DataGridView1.Rows(r).Cells("QUANTITY").Style.ForeColor = Color.Red
                End If
                las = r
            Next
            Label7.Text = "Product Last Update: " & DataGridView1.Rows(las - 1).Cells("Date Updated").Value
            Label1.Text = "Total In The Table: " & tot
            che()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SQLConnection.ConnectionString = serverstring
        loa()
        sear()
        che()
        If typ = "User" Then
            TextBox1.Enabled = False
            TextBox1.Visible = False
            Button2.Enabled = False
            Button2.Visible = False
        Else
            TextBox1.Enabled = True
            TextBox1.Visible = True
            Button2.Enabled = True
            Button2.Visible = True
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim dt As New DataTable
        With dt
            .Columns.Add("quan")
            .Columns.Add("name")
            .Columns.Add("date")
            .Columns.Add("com")
            .Columns.Add("dr")
            .Columns.Add("si")
        End With
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            dt.Rows.Add(DataGridView1.Rows(x).Cells(0).Value, DataGridView1.Rows(x).Cells(1).Value, DataGridView1.Rows(x).Cells(2).Value, DataGridView1.Rows(x).Cells(3).Value, DataGridView1.Rows(x).Cells(4).Value, DataGridView1.Rows(x).Cells(5).Value)
        Next
        Dim rpt As New CrystalReport2
        rpt.SetDataSource(dt)
        rpt.SetParameterValue("code", Label2.Text)
        rpt.SetParameterValue("name", Label3.Text)
        rpt.SetParameterValue("descri", Label4.Text)
        rpt.SetParameterValue("size", Label5.Text)
        rpt.SetParameterValue("quan", Label6.Text)
        rpt.SetParameterValue("date", Label7.Text)
        rpt.SetParameterValue("type", Label8.Text)
        rpt.SetParameterValue("tot", Label1.Text)
        Form4.CrystalReportViewer1.ReportSource = rpt
        Form4.CrystalReportViewer1.Refresh()

        Form4.ShowDialog()
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If TextBox1.Text <> "" Then
                Dim che As Double
                che = che + TextBox1.Text
            End If
        Catch ex As Exception
            MsgBox("Numbers Only")
            TextBox1.Text = ""
        End Try
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim upt As String = "UPDATE scancode.items SET price=" & TextBox1.Text & " Where code = '" & code & "';"
        SQLConnection.Close()
        SQLConnection.Open()
        With cmd
            .CommandText = upt
            .Connection = SQLConnection
            .ExecuteNonQuery()
        End With
        SQLConnection.Close()
        MsgBox("Updated")
        Form1.loa()
    End Sub

    Private Sub TextBox1_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If TextBox2.Text <> "" Then
            If Label10.Text = "Serial No." Then
                PictureBox1.Image = qr.Encode(code & " , " & pn & " , " & Label5.Text)
                un = ""
            End If
            Dim ds As New DataSet1
            Dim ms As New System.IO.MemoryStream
            PictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim byt() As Byte = ms.ToArray
            ds.DataTable2.Rows.Add(byt)
            Dim rpt As New CrystalReport7
            rpt.SetDataSource(ds.Tables("DataTable2"))
            rpt.SetParameterValue("name", Label3.Text)
            rpt.SetParameterValue("size", Label5.Text)
            rpt.SetParameterValue("des", Label4.Text)
            rpt.SetParameterValue("unit", un)
            rpt.SetParameterValue("qua", TextBox2.Text)
            Form4.CrystalReportViewer1.ReportSource = rpt
            Form4.CrystalReportViewer1.Refresh()
            Form4.ShowDialog()
        Else
            MsgBox("Set Quantity Per box")
            TextBox2.Select()
        End If


    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

        Try
            If TextBox2.Text <> "" And Label10.Text <> "Serial No." Then
                Dim a As Double
                a = a + TextBox2.Text

            End If
        Catch ex As Exception
            TextBox2.Text = ""
            TextBox2.Select()
            MsgBox("Numbers Only")
        End Try
        
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim rpt As New CrystalReport8
        rpt.SetParameterValue("name", pn)
        rpt.SetParameterValue("size", "Size: " & ps)
        rpt.SetParameterValue("des", pd)
        rpt.SetParameterValue("unit", un)
        rpt.SetParameterValue("qua", TextBox2.Text)
        Form4.CrystalReportViewer1.ReportSource = rpt
        Form4.CrystalReportViewer1.Refresh()
        Form4.ShowDialog()
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class