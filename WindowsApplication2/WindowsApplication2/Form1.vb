Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Text
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class Form1
    Dim serverstring As String = "Server=192.168.1.32;UserId='scancode';Password='admin101';Database=scancode;Convert Zero Datetime=True"
    Dim SQLConnection As MySqlConnection = New MySqlConnection
    Dim cmd As MySqlCommand = New MySqlCommand
    Dim rer As MySqlDataReader
    Dim formattedDate As String = Date.Today.ToString("yyyy/MM/dd")
    Dim ty As String
    Public panga As String
    Public typ As String
   
    Public Sub noti()
        Dim wew As String = "SELECT count(*) FROM items where quan=0 and unit <> 'Piece';"
        Dim nt As Integer
        cmd = New MySqlCommand(wew, SQLConnection)
        SQLConnection.Open()
        rer = cmd.ExecuteReader
        While rer.Read
            Dim noti = rer.GetInt16("count(*)")
            nt = noti
            Button17.Text = "Notification(" & noti & ")"
            If nt > 0 Then
                Button17.ForeColor = Color.Red
            Else
                Button17.ForeColor = Color.Black
            End If
        End While
        SQLConnection.Close()
    End Sub

    Public Sub prin()
        Dim dt As New DataTable
        With dt
            .Columns.Add("pn")
            .Columns.Add("quan")
            .Columns.Add("size")
            .Columns.Add("des")
            .Columns.Add("un")
        End With
        For x As Integer = 0 To DataGridView2.Rows.Count - 2
            Dim che As Double
            If DataGridView2.Rows(x).Cells(2).Value < 0 Then
                che = DataGridView2.Rows(x).Cells(2).Value * -1
            Else
                che = DataGridView2.Rows(x).Cells(2).Value
            End If
            dt.Rows.Add(DataGridView2.Rows(x).Cells(1).Value, che, DataGridView2.Rows(x).Cells(3).Value, DataGridView2.Rows(x).Cells(6).Value, DataGridView2.Rows(x).Cells(4).Value)
        Next
        Dim rpt As New CrystalReport3
        rpt.SetDataSource(dt)
        rpt.SetParameterValue("comp", TextBox9.Text)
        rpt.SetParameterValue("ad", TextBox10.Text)
        rpt.SetParameterValue("ter", TextBox11.Text & " Day(s)")
        rpt.SetParameterValue("po", TextBox15.Text)

        rpt.SetParameterValue("del", "***  " & ComboBox1.Text & "  ***")
        Form4.CrystalReportViewer1.ReportSource = rpt
        Form4.CrystalReportViewer1.Refresh()
        Form4.ShowDialog()
        ' prin2()
        If ComboBox1.Text = "Partial Delivery" Then
            Button16.Enabled = True
            Button15.Enabled = True
            Button2.Enabled = True
        Else
            Button2.Enabled = False
            Button16.Enabled = True
            Button15.Enabled = True
        End If
       

    End Sub
    Public Sub prin2()
        Dim total As Double
        Dim dt As New DataTable
        With dt
            .Columns.Add("pn")
            .Columns.Add("qua")
            .Columns.Add("siz")
            .Columns.Add("de")
            .Columns.Add("un")
            .Columns.Add("pri")
            .Columns.Add("amount")
        End With
        For x As Integer = 0 To DataGridView2.Rows.Count - 1
            Dim amou As Double
            Dim che As Double
            If DataGridView2.Rows(x).Cells(2).Value < 0 Then
                che = DataGridView2.Rows(x).Cells(2).Value * -1
            Else
                che = DataGridView2.Rows(x).Cells(2).Value
            End If
            amou = che * DataGridView2.Rows(x).Cells(5).Value
            total = total + amou
            If amou = 0 Then
                Exit For
            End If
            dt.Rows.Add(DataGridView2.Rows(x).Cells(1).Value, che, DataGridView2.Rows(x).Cells(3).Value, DataGridView2.Rows(x).Cells(4).Value, DataGridView2.Rows(x).Cells(6).Value, Format(Val(DataGridView2.Rows(x).Cells(5).Value), "#,##0.00"), Format(Val(amou), "#,##0.00"))
        Next
        Dim va As Double

        If CheckBox3.Checked = True Then
            va = TextBox16.Text
        Else
            va = 0
        End If
        Dim upt2 As String = "INSERT INTO sales (si,dr, compa,tin,amo,da,rem,adr,tax,ter,po) Values(" & TextBox12.Text & "," & TextBox13.Text & ",'" & TextBox9.Text & "','" & TextBox14.Text & "'," & total & ",'" & formattedDate & "',' ' ,'" & TextBox10.Text & "'," & va & "," & TextBox11.Text & ",'" & TextBox15.Text & "');"
        SQLConnection.Open()
        With cmd
            .CommandText = upt2
            .Connection = SQLConnection
            .ExecuteNonQuery()
        End With
        SQLConnection.Close()
        Dim vate As String = ""
        Dim Zer As String = ""
        If ComboBox3.Text = "Zero Rated Sales" Then
            Zer = "Php " & Format(Val(total), "#,##0.00")
            vate = ""
        ElseIf ComboBox3.Text = "VAT- Exempt Sales" Then
            vate = "Php " & Format(Val(total), "#,##0.00")
            Zer = ""
        Else
            Zer = ""
            vate = ""
        End If
        Dim rpt As New CrystalReport4
        rpt.SetDataSource(dt)
        rpt.SetParameterValue("vatex", vate)
        rpt.SetParameterValue("zervat", Zer)
        rpt.SetParameterValue("comp", TextBox9.Text)
        rpt.SetParameterValue("tin", TextBox14.Text)
        rpt.SetParameterValue("add", TextBox10.Text)
        rpt.SetParameterValue("po", TextBox15.Text)
        rpt.SetParameterValue("dr", TextBox13.Text)
        rpt.SetParameterValue("terms", TextBox11.Text & " Day(s)")
        rpt.SetParameterValue("total", "Php " & Format(Val(total), "#,##0.00"))
        rpt.SetParameterValue("lvat", "Php " & Label19.Text)
        rpt.SetParameterValue("vat", "Php " & Label18.Text)

        If Label44.Text <> "" Then
            Dim upt3 As String = "UPDATE hist set  si = '" & TextBox12.Text & "' where dr = " & Label44.Text & ";"
            SQLConnection.Close()
            SQLConnection.Open()
            With cmd
                .CommandText = upt3
                .Connection = SQLConnection
                .ExecuteNonQuery()
            End With
            SQLConnection.Close()
        End If
        Form4.CrystalReportViewer1.ReportSource = rpt
        Form4.CrystalReportViewer1.Refresh()
        Form4.ShowDialog()
        DataGridView2.Rows.Clear()
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""
        TextBox12.Text = ""
        TextBox13.Text = ""
        TextBox14.Text = ""
        TextBox15.Text = ""
        Label17.Text = ""
        Label18.Text = ""
        Label19.Text = ""
        Button15.Enabled = False
        Button16.Enabled = False
        Button2.Enabled = True
        loa6()
    End Sub
    Public Sub ad()
        loa2()
        Dim holder As Integer
        For dr = 0 To DataGridView2.RowCount - 1
            If dr = DataGridView2.RowCount - 1 Then
                Exit For
            End If
            If ty = "ADD" Then
                holder = DataGridView2.Rows(dr).Cells("Column4").Value * 1
            ElseIf ty = "Deduct" Then
                holder = DataGridView2.Rows(dr).Cells("Column4").Value * -1
            End If
            Dim cmd As MySqlCommand = New MySqlCommand
            If ComboBox1.Text = "Partial Delivery" And TextBox12.Text = "" Then
                TextBox12.Text = ComboBox1.Text
            End If
            Dim upt2 As String = "INSERT INTO hist(code, quan, name, dat, dr, si, company, price,size,descri,unit,pnme) Values('" & DataGridView2.Rows(dr).Cells("Column2").Value & "','" & holder & "','" & panga & "','" & formattedDate & "'," & TextBox13.Text & ",'" & TextBox12.Text & "','" & TextBox9.Text & "'," & DataGridView2.Rows(dr).Cells("price").Value & ",'" & DataGridView2.Rows(dr).Cells("Size").Value & "','" & DataGridView2.Rows(dr).Cells("des").Value & "','" & DataGridView2.Rows(dr).Cells("unit").Value & "','" & DataGridView2.Rows(dr).Cells("Column3").Value & "');"
            SQLConnection.Close()
            SQLConnection.Open()
            With cmd
                .CommandText = upt2
                .Connection = SQLConnection
                .ExecuteNonQuery()
            End With
            SQLConnection.Close()
            Dim upt3 As String = "UPDATE hist set dr= '', si = '' where dr = 0;"
            SQLConnection.Close()
            SQLConnection.Open()
            With cmd
                .CommandText = upt3
                .Connection = SQLConnection
                .ExecuteNonQuery()
            End With
            SQLConnection.Close()
        Next
        loa()
    End Sub
    Public Sub loa3()
        Try
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT sales.compa as 'Company',sales.tin as 'TIN No.', sales.po as 'Po No.',sales.si as 'Sales Invoice', sales.dr as 'Delivery Reciept',sales.ter as 'Terms', hist.code as 'Code',hist.pnme as 'Product name' , hist.size as 'Size', hist.descri as 'Description', hist.price as 'Price',hist.quan as 'Quantity', hist.unit as 'Unit', (hist.quan* hist.price)*-1 as 'Amount', (sales.tax/100)*((hist.quan* hist.price)/1.12)*-1 as 'Vat',((hist.quan* hist.price)*-1)-((sales.tax/100)*((hist.quan* hist.price)/1.12)*-1) as 'Net of Vat' , sales.da as 'Date', hist.name as 'Created By',sales.rem as 'Remarks' FROM sales, hist where hist.si = sales.si and hist.dr = sales.dr and hist.company= sales.compa and sales.da between '" & DateTimePicker1.Text & "' and '" & DateTimePicker2.Text & "' ;", serverstring)
            adapter.Fill(table)
            DataGridView3.DataSource = table
            DataGridView3.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            SQLConnection.Close()
        Catch ex As Exception
            SQLConnection.Close()
        End Try
    End Sub
    Public Sub loa4()
        Try
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT h.code as 'Code', h.quan as 'Quantity', h.name as 'Updated By', h.dat as 'Date', h.company as 'Company', h.dr as 'D.R.', h.si as 'S.I.' FROM hist h where h.dat between '" & DateTimePicker1.Text & "' and '" & DateTimePicker2.Text & "' ;", serverstring)
            adapter.Fill(table)
            DataGridView4.DataSource = table
            DataGridView4.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            SQLConnection.Close()
        Catch ex As Exception
            SQLConnection.Close()
        End Try
    End Sub
    Public Sub loa2()
        Try
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT * FROM hist h;", serverstring)
            adapter.Fill(table)
            DataGridView1.DataSource = table
            DataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            SQLConnection.Close()
        Catch ex As Exception
            SQLConnection.Close()
        End Try
    End Sub
    Public Sub loa6()
        Try
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT si as 'Sales Invoice', dr as 'Delivery Reciept', compa as 'Company', tin  as 'Company Tin', amo as 'Amount', da as 'Date' FROM sales s where rem='' ;", serverstring)
            adapter.Fill(table)
            DataGridView5.DataSource = table
            DataGridView5.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            SQLConnection.Close()
        Catch ex As Exception
            SQLConnection.Close()
        End Try
    End Sub
    Public Sub ser2()
        Try
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT si as 'Sales Invoice', dr as 'Delivery Reciept', compa as 'Company', tin  as 'Company Tin', amo as 'Amount', da as 'Date' FROM sales s where rem='' and  si like '%" & TextBox17.Text & "%' or dr like '%" & TextBox17.Text & "%' or compa like '%" & TextBox17.Text & "%' or amo like '%" & TextBox17.Text & "%' ;", serverstring)
            adapter.Fill(table)
            DataGridView5.DataSource = table
            SQLConnection.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
            SQLConnection.Close()
        End Try
    End Sub

    Public Sub upd()
        loa()
        For dr = 0 To DataGridView2.RowCount - 1
            For t = 0 To DataGridView1.RowCount - 1
                If DataGridView2.Rows(dr).Cells("Column2").Value = DataGridView1.Rows(t).Cells("CODE").Value Then
                    Dim upt As String = "UPDATE scancode.items SET quan=" & DataGridView1.Rows(t).Cells("QUANTITY").Value - DataGridView2.Rows(dr).Cells("Column4").Value & ",dat='" & formattedDate & "' WHERE code='" & DataGridView2.Rows(dr).Cells("Column2").Value & "';"
                    SQLConnection.Close()
                    SQLConnection.Open()
                    With cmd
                        .CommandText = upt
                        .Connection = SQLConnection
                        .ExecuteNonQuery()
                    End With
                    SQLConnection.Close()
                    Exit For
                Else
                End If
            Next
        Next
        MsgBox("UPDATE SUCCESS")
        ad()
    End Sub
    Public Sub tryc()
        Try
            If SQLConnection.State = ConnectionState.Closed Then
                SQLConnection.Open()
                MsgBox("Connect")
            Else
                SQLConnection.Close()
            End If
            SQLConnection.Close()
        Catch ex As Exception
            SQLConnection.Close()
            MsgBox("Connection Lost. Disconnected from server.Please check your LAN cable, Router ,Or WI-FI Connection.Then Click Ok.")
        End Try
    End Sub
    Public Sub loa()
        Try
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT i.code as 'CODE', i.name as 'NAME' , i.descri as 'DESCRIPTION', i.size as 'SIZE',i.typ as 'Type', i.quan as 'QUANTITY',i.dat as 'Last Update',i.unit as 'UNIT', i.price as 'PRICE' FROM items i;", serverstring)
            adapter.Fill(table)
            DataGridView1.DataSource = table
            DataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            SQLConnection.Close()
            If typ = "User" Then
                DataGridView1.Columns("PRICE").Visible = False
            Else
                DataGridView1.Columns("PRICE").Visible = True
               
            End If
            loa6()
            noti()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub sear()
        Try
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT i.code as 'CODE', i.name as 'NAME' , i.descri as 'DESCRIPTION', i.size as 'SIZE',i.typ as 'Type', i.quan as 'QUANTITY',i.dat as 'Last Update',i.unit as 'UNIT', i.price as 'PRICE' FROM items i where i.code like '%" & TextBox1.Text & "%' or i.descri like '%" & TextBox1.Text & "%' or i.name like '%" & TextBox1.Text & "%' or i.size like '%" & TextBox1.Text & "%' or i.typ like '%" & TextBox1.Text & "%'  ;", serverstring)
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

            SQLConnection.Close()
        End Try
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SQLConnection.ConnectionString = serverstring
        Label8.Visible = False
        Button7.Enabled = False
        CheckBox2.Checked = True
        DateTimePicker1.Format = DateTimePickerFormat.Custom
        DateTimePicker1.CustomFormat = "yyyy/MM/dd"
        DateTimePicker2.Format = DateTimePickerFormat.Custom
        DateTimePicker2.CustomFormat = "yyyy/MM/dd"
        DateTimePicker1.Value = formattedDate
        DateTimePicker2.Value = formattedDate
        TextBox16.Text = 12
        TextBox16.Enabled = False
        Button13.Enabled = False
        Button14.Enabled = False
        TabControl1.SelectedTab = TabPage1

        If typ = "User" Then
            CheckBox2.Enabled = False
            Button5.Enabled = False
            Button12.Enabled = False
            DataGridView5.Visible = False
            DataGridView6.Visible = False
            DataGridView7.Visible = False
            Button13.Enabled = False
            Button10.Enabled = False
            Button11.Enabled = False
            Button7.Enabled = False
            CheckBox1.Enabled = False
            
           
        Else
            CheckBox2.Enabled = True
            Button5.Enabled = True
            Button12.Enabled = True
            DataGridView5.Visible = True
            DataGridView6.Visible = True
            DataGridView7.Visible = True
            Button13.Enabled = True
            Button10.Enabled = True
            Button11.Enabled = True
            Button7.Enabled = True
            CheckBox1.Enabled = True
           
            
        End If
        If typ = "User" Then
            DataGridView2.Columns("PRICE").Visible = False
        Else
            DataGridView2.Columns("PRICE").Visible = True

        End If

        'tryc()
        loa()
        loa6()


    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            sear()
        Else
            loa()
        End If
    End Sub
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            Dim row As DataGridViewRow
            row = Me.DataGridView1.Rows(e.RowIndex)
            Dim b As String
            Dim c As Integer
            b = row.Cells("CODE").Value
            For dr = 0 To DataGridView2.RowCount - 1
                If b = DataGridView2.Rows(dr).Cells("Column2").Value Then
                    Exit For
                End If
                c = dr
            Next
            If c = DataGridView2.RowCount - 1 Then
                'DataGridView2.Rows.Add(row.Cells("CODE").Value, row.Cells("NAME").Value, row.Cells("QUANTITY").Value, row.Cells("SIZE").Value, row.Cells("Description").Value, row.Cells("Price").Value, row.Cells("Unit").Value)
            End If
            DataGridView2.Rows.Add(row.Cells("CODE").Value, row.Cells("NAME").Value, row.Cells("QUANTITY").Value, row.Cells("SIZE").Value, row.Cells("Description").Value, row.Cells("Price").Value, row.Cells("Unit").Value)
            TextBox13.Text = 0
            TextBox12.Text = 0
            TextBox11.Text = 30
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If TextBox13.Text <> "" And TextBox9.Text <> "" And TextBox10.Text <> "" And TextBox11.Text <> "" And TextBox13.Text <> "" Then
            Dim check As Integer
            ty = "Deduct"
            If DataGridView2.RowCount - 1 = 0 Then
                MsgBox("No Data")
            Else
                For r = 0 To DataGridView2.RowCount - 1
                    DataGridView2.Rows(r).Cells("Column4").Style.BackColor = Color.White
                    DataGridView2.Rows(r).Cells("Column2").Style.BackColor = Color.White
                    DataGridView2.Rows(r).Cells("Column3").Style.BackColor = Color.White
                    Dim wew As String = "SELECT i.code,i.name,i.quan FROM items i where i.code= '" & DataGridView2.Rows(r).Cells("Column2").Value & "';"
                    cmd = New MySqlCommand(wew, SQLConnection)
                    SQLConnection.Close()
                    SQLConnection.Open()
                    rer = cmd.ExecuteReader
                    While rer.Read
                        Dim name = rer.GetString("code")
                        Dim pas = rer.GetString("name")
                        Dim id = rer.GetInt32("quan")
                        check = DataGridView2.Rows(r).Cells("Column4").Value
                        If check > id Or id = 0 Then
                            MsgBox("The Out Going Stocks Should not be higher than actual stocks Your Input: " & check & " In Stock: " & id & " Code: " & DataGridView2.Rows(r).Cells("Column2").Value)
                            DataGridView2.Rows(r).Cells("Column4").Style.BackColor = Color.Red
                            DataGridView2.Rows(r).Cells("Column2").Style.BackColor = Color.Red
                            DataGridView2.Rows(r).Cells("Column3").Style.BackColor = Color.Red
                            Exit For
                        End If
                    End While
                    SQLConnection.Close()
                    check = r
                Next
                If check = DataGridView2.RowCount - 1 Then
                    upd()
                    prin()
                    Button2.Enabled = False
                End If


            End If
        Else
            MsgBox("Empty Fields")
        End If
        


    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox9.Text <> "" Then
            If DataGridView2.RowCount - 1 = 0 Then
                MsgBox("No Data")
            Else
                loa()
                ty = "ADD"
                For dr = 0 To DataGridView2.RowCount - 1
                    For t = 0 To DataGridView1.RowCount - 1
                        If DataGridView2.Rows(dr).Cells("Column2").Value = DataGridView1.Rows(t).Cells("CODE").Value Then
                            Dim upt As String = "UPDATE scancode.items SET quan=" & DataGridView1.Rows(t).Cells("QUANTITY").Value + DataGridView2.Rows(dr).Cells("Column4").Value & ",dat='" & formattedDate & "' WHERE code='" & DataGridView2.Rows(dr).Cells("Column2").Value & "';"
                            SQLConnection.Close()
                            SQLConnection.Open()
                            With cmd
                                .CommandText = upt
                                .Connection = SQLConnection
                                .ExecuteNonQuery()
                            End With
                            SQLConnection.Close()
                            Exit For
                        Else
                        End If
                    Next
                Next
                ad()
                DataGridView2.Rows.Clear()
                TextBox9.Text = ""
                TextBox10.Text = ""
                TextBox11.Text = ""
                TextBox12.Text = ""
                TextBox13.Text = ""
                TextBox14.Text = ""
                TextBox15.Text = ""
                MsgBox("UPDATE SUCCESS")

            End If
        Else
            MsgBox("Empty Fields")
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Try
            Dim row As DataGridViewRow
            row = Me.DataGridView1.Rows(e.RowIndex)
            DataGridView2.Rows.Clear()
            SQLConnection.Close()
            TextBox13.Text = 0
            TextBox12.Text = 0
            Form2.code = row.Cells("CODE").Value
            Form2.siz = row.Cells("SIZE").Value
            Form2.Show()
            Form2.che()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        DataGridView2.Rows.Clear()
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""
        TextBox12.Text = ""
        TextBox13.Text = ""
        TextBox14.Text = ""
        TextBox15.Text = ""
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        loa()
        loa6()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Form3.ShowDialog()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If typ = "User" Then
            Dim dt As New DataTable
            With dt
                .Columns.Add("cod")
                .Columns.Add("nam")
                .Columns.Add("des")
                .Columns.Add("size")
                .Columns.Add("quan")
                .Columns.Add("type")
            End With
            For x As Integer = 0 To DataGridView1.Rows.Count - 1
                dt.Rows.Add(DataGridView1.Rows(x).Cells(0).Value, DataGridView1.Rows(x).Cells(1).Value, DataGridView1.Rows(x).Cells(2).Value, DataGridView1.Rows(x).Cells(3).Value, DataGridView1.Rows(x).Cells(4).Value, DataGridView1.Rows(x).Cells(5).Value)
            Next
            Dim rpt As New CrystalReport1
            rpt.SetDataSource(dt)
            rpt.SetParameterValue("name", panga)
            Form4.CrystalReportViewer1.ReportSource = rpt
            Form4.CrystalReportViewer1.Refresh()
            Form4.ShowDialog()
        Else
            Dim dt As New DataTable
            With dt
                .Columns.Add("cod")
                .Columns.Add("nam")
                .Columns.Add("des")
                .Columns.Add("size")
                .Columns.Add("quan")
                .Columns.Add("type")
                .Columns.Add("price")
            End With
            For x As Integer = 0 To DataGridView1.Rows.Count - 1
                dt.Rows.Add(DataGridView1.Rows(x).Cells(0).Value, DataGridView1.Rows(x).Cells(1).Value, DataGridView1.Rows(x).Cells(2).Value, DataGridView1.Rows(x).Cells(3).Value, DataGridView1.Rows(x).Cells(4).Value, DataGridView1.Rows(x).Cells(5).Value, DataGridView1.Rows(x).Cells(8).Value)
            Next
            Dim rpt As New CrystalReport9
            rpt.SetDataSource(dt)
            rpt.SetParameterValue("name", panga)
            Form4.CrystalReportViewer1.ReportSource = rpt
            Form4.CrystalReportViewer1.Refresh()
            Form4.ShowDialog()
        End If
      
    End Sub

   

    Private Sub TextBox11_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles TextBox11.PreviewKeyDown
        If e.KeyData = Keys.Tab Then
            e.IsInputKey = True
            CheckBox3.Select()
        End If
    End Sub
    Private Sub TextBox11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox11.TextChanged
        Try
            If TextBox11.Text <> "" Then
                Dim che As Double
                che = che + TextBox11.Text

            End If
        Catch ex As Exception
            MsgBox("Numbers Only!")
            TextBox11.Text = ""
            TextBox11.Focus()
        End Try
        
    End Sub
    Private Sub TextBox12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox12.TextChanged
        Dim charactersAllowed As String = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.,1234567890"
        Dim theText As String = TextBox12.Text
        Dim Letter As String
        For x As Integer = 0 To TextBox12.Text.Length - 1
            Letter = TextBox12.Text.Substring(x, 1)
            If charactersAllowed.Contains(Letter) = False Then
                theText = theText.Replace(Letter, String.Empty)
            End If
        Next
        TextBox12.Text = theText
        TextBox12.Select(TextBox12.Text.Length, 0)
    End Sub

    Private Sub TextBox13_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles TextBox13.PreviewKeyDown
        If e.KeyData = Keys.Tab Then
            e.IsInputKey = True
            TextBox11.Select()
        End If
    End Sub

    Private Sub TextBox13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox13.TextChanged
        Try
            If TextBox13.Text <> "" Then
                Dim che As Double
                che = che + TextBox13.Text

            End If
        Catch ex As Exception
            MsgBox("Numbers Only!")
            TextBox13.Text = ""
            TextBox13.Focus()
        End Try
    End Sub

    Private Sub DataGridView3_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles DataGridView3.CellFormatting
        DataGridView3.Columns.Item("Amount").DefaultCellStyle.Format = "n2"
        DataGridView3.Columns.Item("Vat").DefaultCellStyle.Format = "n2"
        DataGridView3.Columns.Item("Net of Vat").DefaultCellStyle.Format = "n2"
    End Sub

    Private Sub DataGridView2_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellEndEdit
        If CheckBox3.Checked = True Then
            Dim total As Double
            For x As Integer = 0 To DataGridView2.Rows.Count - 1
                Dim amou As Double
                Dim che As Double
                If DataGridView2.Rows(x).Cells(2).Value < 0 Then
                    che = DataGridView2.Rows(x).Cells(2).Value * -1
                Else
                    che = DataGridView2.Rows(x).Cells(2).Value
                End If
                amou = che * DataGridView2.Rows(x).Cells(5).Value
                total = total + amou
                If amou = 0 Then
                    Exit For
                End If
                Label17.Text = Format(Val(total), "#,##0.00")
                Dim net As Double
                net = total / 1.12
                Label18.Text = Format(Val(net * (TextBox16.Text / 100)), "#,##0.00")
                Dim lv As Double
                lv = total - net * (TextBox16.Text / 100)
                Label19.Text = Format(Val(lv), "#,##0.00")
                If CheckBox2.Checked = True Then
                    Label18.Text = ""
                    Label19.Text = ""
                End If
            Next

        End If
    End Sub

    Private Sub DataGridView2_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles DataGridView2.CellFormatting
        DataGridView2.Columns.Item("price").DefaultCellStyle.Format = "n2"
    End Sub

    Private Sub DataGridView1_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        DataGridView1.Columns.Item("PRICE").DefaultCellStyle.Format = "n2"
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            Button2.Enabled = False
            Button1.Enabled = True
            TextBox10.Enabled = False
            TextBox11.Enabled = False
            TextBox12.Enabled = False
            TextBox13.Enabled = False
            TextBox14.Enabled = False
            TextBox15.Enabled = False
            ComboBox3.Enabled = False
            Button15.Enabled = False
            Button16.Enabled = False
            TextBox13.Text = 0
            TextBox12.Text = 0
            CheckBox3.Enabled = False
            Label17.Text = ""
            Label18.Text = ""
            Label19.Text = ""
            ComboBox1.Enabled = False
        Else
            TextBox10.Enabled = True
            TextBox11.Enabled = True
            TextBox12.Enabled = True
            TextBox13.Enabled = True
            TextBox14.Enabled = True
            TextBox15.Enabled = True
            Button2.Enabled = True
            Button1.Enabled = False
            CheckBox3.Enabled = True
            ComboBox1.Enabled = True
            ComboBox3.Enabled = True

        End If
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim dt As New DataTable
        With dt
            .Columns.Add("si")
            .Columns.Add("dr")
            .Columns.Add("compa")
            .Columns.Add("tin")
            .Columns.Add("amo")
            .Columns.Add("da")
            .Columns.Add("rem")
        End With
        Dim tot As Double
        tot = 0
        For x As Integer = 0 To DataGridView3.Rows.Count - 1
            tot = tot + DataGridView3.Rows(x).Cells(15).Value
            dt.Rows.Add(DataGridView3.Rows(x).Cells(3).Value, DataGridView3.Rows(x).Cells(4).Value, DataGridView3.Rows(x).Cells(0).Value, DataGridView3.Rows(x).Cells(1).Value, DataGridView3.Rows(x).Cells(15).Value, DataGridView3.Rows(x).Cells(16).Value, DataGridView3.Rows(x).Cells(18).Value)
        Next
        Dim rpt As New CrystalReport5
        rpt.SetDataSource(dt)
        rpt.SetParameterValue("TOT", "Total: Php" & Format(Val(tot), "#,##0.00"))
        Form4.CrystalReportViewer1.ReportSource = rpt
        Form4.CrystalReportViewer1.Refresh()
        Form4.ShowDialog()
       

    End Sub
   
    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            TextBox16.Enabled = True
            ComboBox3.Enabled = False
            ComboBox3.Text = ""
            Dim total As Double
            For x As Integer = 0 To DataGridView2.Rows.Count - 1
                Dim amou As Double
                Dim che As Double
                If DataGridView2.Rows(x).Cells(2).Value < 0 Then
                    che = DataGridView2.Rows(x).Cells(2).Value * -1
                Else
                    che = DataGridView2.Rows(x).Cells(2).Value
                End If
                amou = che * DataGridView2.Rows(x).Cells(5).Value
                total = total + amou
                If amou = 0 Then
                    Exit For
                End If
                Label17.Text = Format(Val(total), "#,##0.00")
                Dim net As Double
                net = total / 1.12
                Label18.Text = Format(Val(net * (TextBox16.Text / 100)), "#,##0.00")
                Dim lv As Double
                lv = total - net * (TextBox16.Text / 100)
                Label19.Text = Format(Val(lv), "#,##0.00")
            Next
        Else
            TextBox16.Enabled = False
            TextBox16.Text = 12
            Label18.Text = ""
            Label19.Text = ""
            ComboBox3.Enabled = True
            ComboBox3.Text = "Zero Rated Sales"
        End If
    End Sub

    Private Sub TextBox16_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox16.TextChanged
        Try
            Dim qwe As Double
            qwe = qwe + TextBox16.Text
            Dim total As Double
            For x As Integer = 0 To DataGridView2.Rows.Count - 1
                Dim amou As Double
                amou = DataGridView2.Rows(x).Cells(2).Value * DataGridView2.Rows(x).Cells(5).Value
                total = total + amou
                If amou = 0 Then
                    Exit For
                End If
                Label17.Text = Format(Val(total), "#,##0.00")
                Label18.Text = Format(Val(total * (TextBox16.Text / 100)), "#,##0.00")
                Dim lv As Double
                lv = total - (total * TextBox16.Text / 100)
                Label19.Text = Format(Val(lv), "#,##0.00")
                If CheckBox2.Checked = True Then
                    Label17.Text = ""
                    Label18.Text = ""
                    Label19.Text = ""
                End If
            Next
        Catch ex As Exception
            TextBox16.Text = 12
            MsgBox("Numbers only")

        End Try


    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Dim dt As New DataTable
        With dt
            .Columns.Add("code")
            .Columns.Add("qua")
            .Columns.Add("up")
            .Columns.Add("da")
            .Columns.Add("com")
            .Columns.Add("dr")
            .Columns.Add("si")
        End With
        For x As Integer = 0 To DataGridView4.Rows.Count - 1
            dt.Rows.Add(DataGridView4.Rows(x).Cells(0).Value, DataGridView4.Rows(x).Cells(1).Value, DataGridView4.Rows(x).Cells(2).Value, DataGridView4.Rows(x).Cells(3).Value, DataGridView4.Rows(x).Cells(4).Value, DataGridView4.Rows(x).Cells(5).Value, DataGridView4.Rows(x).Cells(6).Value)
        Next
        Dim rpt As New CrystalReport6
        rpt.SetDataSource(dt)
        Form4.CrystalReportViewer1.ReportSource = rpt
        Form4.CrystalReportViewer1.Refresh()
        Form4.ShowDialog()

    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Try
            Dim file As String
            SaveFileDialog1.Filter = "SQL Dump File (*.sql)|*.sql|All files (*.*)|*.*"
            SaveFileDialog1.FileName = DateTime.Now.ToString("yyyy-MM-dd") + ".sql"
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                file = SaveFileDialog1.FileName
                'Process.Start("C:\Users\b3ngz\Documents\Visual Studio 2010\Projects\WindowsApplication2\bak2.bat", file)
                'Process.Start("C:\Program Files (x86)\MySQL\MySQL Server 5.5\bin\mysqldump.exe", "-h Ryan -u root -ppacifico thesis>" & file & "")
                'MsgBox("Back up Complete!.It is in the D Drive and named 'singleDbBackup.sql'!", MsgBoxStyle.Information, "Back up")
                Dim myProcess As New Process()
                myProcess.StartInfo.FileName = "cmd.exe"
                myProcess.StartInfo.UseShellExecute = False
                myProcess.StartInfo.WorkingDirectory = "C:\Program Files (x86)\MySQL\MySQL Server 5.5\bin"
                myProcess.StartInfo.RedirectStandardInput = True
                myProcess.StartInfo.RedirectStandardOutput = True
                myProcess.Start()
                Dim myStreamWriter As StreamWriter = myProcess.StandardInput
                Dim mystreamreader As StreamReader = myProcess.StandardOutput
                myStreamWriter.WriteLine("mysqldump -h 192.168.1.32 -u scancode -padmin101 scancode> " & file & "")
                myStreamWriter.Close()
                myProcess.WaitForExit()
                myProcess.Close()
                MsgBox("Backup Created Successfully!", MsgBoxStyle.Information, "Backup")
            End If
        Catch ex As Exception

        End Try
        
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Try
            Dim file As String
            OpenFileDialog1.Filter = "SQL Dump File (*.sql)|*.sql|All files (*.*)|*.*"
            If OpenFileDialog1.ShowDialog = DialogResult.OK Then
                file = OpenFileDialog1.FileName
                Dim myProcess As New Process()
                myProcess.StartInfo.FileName = "cmd.exe"
                myProcess.StartInfo.UseShellExecute = False
                myProcess.StartInfo.WorkingDirectory = "C:\Program Files (x86)\MySQL\MySQL Server 5.5\bin"
                myProcess.StartInfo.RedirectStandardInput = True
                myProcess.StartInfo.RedirectStandardOutput = True
                myProcess.Start()
                Dim myStreamWriter As StreamWriter = myProcess.StandardInput
                Dim mystreamreader As StreamReader = myProcess.StandardOutput
                myStreamWriter.WriteLine("mysql -h 192.168.1.32 -u scancode -padmin101 scancode< " & file & "")
                myStreamWriter.Close()
                myProcess.WaitForExit()
                myProcess.Close()
                MsgBox("Database Restoration Successfully!", MsgBoxStyle.Information, "Restore")
            End If
        Catch ex As Exception

        End Try
        
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        loa3()
        loa4()

    End Sub

    Private Sub TextBox10_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles TextBox10.PreviewKeyDown
        If e.KeyData = Keys.Tab Then
            e.IsInputKey = True
            TextBox14.Select()
        End If
    End Sub

    Private Sub TextBox15_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles TextBox15.PreviewKeyDown
        If e.KeyData = Keys.Tab Then
            e.IsInputKey = True
            TextBox12.Select()
        End If
    End Sub

   
    Private Sub DataGridView5_CellClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView5.CellClick
        Try
            DataGridView7.Rows.Clear()
            Dim row As DataGridViewRow
            row = Me.DataGridView5.Rows(e.RowIndex)
            Label32.Text = row.Cells("Sales Invoice").Value
            Label33.Text = row.Cells("Delivery Reciept").Value
            Label34.Text = row.Cells("Company").Value
            Label35.Text = row.Cells("Company Tin").Value
            Label36.Text = Format(Val(row.Cells("Amount").Value), "#,##0.00")
            Label37.Text = row.Cells("Date").Value
            SQLConnection.Open()
            Dim table As New DataTable()
            Dim adapter As New MySqlDataAdapter("SELECT code as 'Product Code', quan as 'Quantity', dat as 'Date' ,price as 'Price' FROM hist h where si =" & Label32.Text & ";", serverstring)
            adapter.Fill(table)
            DataGridView6.DataSource = table
            DataGridView6.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            SQLConnection.Close()
        Catch ex As Exception
            SQLConnection.Close()
            MsgBox(ex.ToString)
        End Try

        For dr = 0 To DataGridView6.RowCount - 1
            Dim wew As String = "SELECT * FROM items i where i.code= '" & DataGridView6.Rows(dr).Cells("Product Code").Value & "';"
            cmd = New MySqlCommand(wew, SQLConnection)
            SQLConnection.Close()
            SQLConnection.Open()
            rer = cmd.ExecuteReader
            While rer.Read
                Dim quan = rer.GetString("quan")
                DataGridView7.Rows.Add(DataGridView6.Rows(dr).Cells("Product Code").Value, quan)
            End While
            SQLConnection.Close()
        Next
    End Sub



    Private Sub TextBox18_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox18.TextChanged
        If TextBox18.Text <> "" And Label32.Text <> "" Then
            Button13.Enabled = True
        Else
            Button13.Enabled = False

        End If
    End Sub

    Private Sub DataGridView5_CellFormatting1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles DataGridView5.CellFormatting
        DataGridView5.Columns.Item("Amount").DefaultCellStyle.Format = "n2"
    End Sub

    Private Sub Button13_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        For dr = 0 To DataGridView6.RowCount - 1
            If dr = DataGridView6.RowCount - 1 Then
                Exit For
            End If
            Dim upt2 As String = "INSERT INTO hist(code, quan, name, dat, dr, si, company, price,size,descri,unit,pnme) Values('" & DataGridView6.Rows(dr).Cells("Product Code").Value & "','" & DataGridView6.Rows(dr).Cells("Quantity").Value * -1 & "','" & panga & "','" & formattedDate & "'," & Label33.Text & ",'" & Label32.Text & "',' Cancelled Si:" & Label33.Text & " DR: " & Label32.Text & "',0,'','','','');"
            'Dim upt2 As String = "INSERT INTO hist(code, quan, name, dat, dr, si, company, price,size,descri,unit,pnme) Values('" & DataGridView6.Rows(dr).Cells("Product Code").Value.ToString & "'," & DataGridView6.Rows(dr).Cells("Quantity").Value * -1 & ",'" & panga & "','" & formattedDate & "','" & Label33.Text & "','" & Label32.Text & "',' Cancelled Si:" & Label33.Text & " DR: " & Label32.Text & "'," & 0 & ",'','','','','');"
            SQLConnection.Close()
            SQLConnection.Open()
            With cmd
                .CommandText = upt2
                .Connection = SQLConnection
                .ExecuteNonQuery()
            End With
            SQLConnection.Close()
        Next
        For dr = 0 To DataGridView6.RowCount - 1
            If dr = DataGridView6.RowCount - 1 Then
                Exit For
            End If
            For r = 0 To DataGridView7.RowCount - 1
                If DataGridView6.Rows(dr).Cells("Product Code").Value = DataGridView7.Rows(r).Cells("Column1").Value Then
                    Dim upt3 As String = "UPDATE scancode.items SET quan=" & (DataGridView6.Rows(dr).Cells("Quantity").Value * -1) + DataGridView7.Rows(dr).Cells("Column5").Value & ",dat='" & formattedDate & "' WHERE code='" & DataGridView7.Rows(dr).Cells("Column1").Value & "';"
                    SQLConnection.Close()
                    SQLConnection.Open()
                    With cmd
                        .CommandText = upt3
                        .Connection = SQLConnection
                        .ExecuteNonQuery()
                    End With
                    SQLConnection.Close()
                End If
            Next
        Next
        Dim upt As String = "UPDATE scancode.sales SET amo=" & 0 & ",rem ='Cancelled " & formattedDate & "," & TextBox18.Text & "' WHERE si='" & Label32.Text & "' and dr = '" & Label33.Text & "';"
        SQLConnection.Close()
        SQLConnection.Open()
        With cmd
            .CommandText = upt
            .Connection = SQLConnection
            .ExecuteNonQuery()
        End With
        SQLConnection.Close()
        DataGridView7.Rows.Clear()
        Label32.Text = ""
        Label33.Text = ""
        Label34.Text = ""
        Label35.Text = ""
        Label36.Text = ""
        Label37.Text = ""
        Button13.Enabled = False
        loa6()
        loa()
        MsgBox("Cancellation Success!")
    End Sub

    Private Sub TextBox17_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox17.TextChanged
        If TextBox17.Text <> "" Then
            ser2()
        Else
            loa6()

        End If
    End Sub

   
    Private Sub Button7_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox7.Text <> "" Then
            If TextBox7.Text <> TextBox8.Text Then
                MsgBox("Password Not Match")
            Else
                Dim pa As String = TextBox2.Text & " " & TextBox3.Text & " " & TextBox4.Text
                Dim upt2 As String = "INSERT INTO `user` (id, name, pass,username,type) Values(" & TextBox5.Text & ",'" & pa & "','" & TextBox7.Text & "','" & TextBox6.Text & "','" & ComboBox2.Text & "');"
                SQLConnection.Open()
                With cmd
                    .CommandText = upt2
                    .Connection = SQLConnection
                    .ExecuteNonQuery()
                End With
                SQLConnection.Close()
                Button2.Enabled = False
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""
                TextBox6.Text = ""
                TextBox7.Text = ""
                TextBox8.Text = ""
                MsgBox("Success!")
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If TextBox7.PasswordChar = "*" Then
            TextBox7.PasswordChar = ""
            TextBox8.PasswordChar = ""
        Else
            TextBox7.PasswordChar = "*"
            TextBox8.PasswordChar = "*"
        End If
    End Sub

   
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox7.Text <> "" And TextBox8.Text <> "" Then
            Button7.Enabled = True
        Else
            Button7.Enabled = False
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox7.Text <> "" And TextBox8.Text <> "" Then
            Button7.Enabled = True
        Else
            Button7.Enabled = False
        End If
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox7.Text <> "" And TextBox8.Text <> "" Then
            Button7.Enabled = True
        Else
            Button7.Enabled = False
        End If
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        If TextBox5.Text <> "" Then
            Try
                Dim che As Double
                che = che + TextBox5.Text
                If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox7.Text <> "" And TextBox8.Text <> "" Then
                    Button7.Enabled = True
                Else
                    Button7.Enabled = False
                End If

            Catch ex As Exception
                MsgBox("Numbers only!")
                TextBox5.Text = ""
                TextBox5.Focus()
            End Try
        End If
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox7.Text <> "" And TextBox8.Text <> "" Then
            Button7.Enabled = True
        Else
            Button7.Enabled = False
        End If
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged
        If TextBox7.Text <> TextBox8.Text Then
            Label8.Visible = True
        Else
            Label8.Visible = False
        End If
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox7.Text <> "" And TextBox8.Text <> "" Then
            Button7.Enabled = True
        Else
            Button7.Enabled = False
        End If
    End Sub

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged
        If TextBox7.Text <> TextBox8.Text Then
            Label8.Visible = True
        Else
            Label8.Visible = False
        End If
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox7.Text <> "" And TextBox8.Text <> "" Then
            Button7.Enabled = True
        Else
            Button7.Enabled = False
        End If
    End Sub

   

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Dim pase As String
        Dim iden As String
        Dim wew As String = "SELECT * FROM `user` where name= '" & panga & "';"
        cmd = New MySqlCommand(wew, SQLConnection)
        SQLConnection.Open()
        rer = cmd.ExecuteReader
        While rer.Read
            Dim pas = rer.GetString("pass")
            Dim ide = rer.GetString("id")
            pase = pas
            iden = ide
        End While
        SQLConnection.Close()
        If pase = TextBox19.Text Then
            Dim upt3 As String = "UPDATE `user` set pass= '" & TextBox21.Text & "' where id = " & iden & " and name = '" & panga & "';"
            SQLConnection.Close()
            SQLConnection.Open()
            With cmd
                .CommandText = upt3
                .Connection = SQLConnection
                .ExecuteNonQuery()
            End With
            SQLConnection.Close()
            TextBox19.Text = ""
            TextBox20.Text = ""
            TextBox21.Text = ""
            MsgBox("Success!. Password has been Updated Logging out now!")
            Me.Close()
        End If
    End Sub

    Private Sub TextBox20_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox20.TextChanged
        If TextBox20.Text = TextBox21.Text Then
            Button14.Enabled = True
        Else
            Button14.Enabled = False
        End If
    End Sub

    Private Sub TextBox21_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox21.TextChanged
        If TextBox20.Text = TextBox21.Text Then
            Button14.Enabled = True
        Else
            Button14.Enabled = False
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            TextBox21.PasswordChar = ""
            TextBox20.PasswordChar = ""
            TextBox19.PasswordChar = ""
        Else
            TextBox21.PasswordChar = "*"
            TextBox20.PasswordChar = "*"
            TextBox19.PasswordChar = "*"
        End If
    End Sub

    Private Sub TextBox9_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox9.LostFocus
        TextBox14.Text = ""
        TextBox10.Text = ""
        Try
            If TextBox9.Text <> "" Then
                Dim wew As String = "SELECT * FROM sales where compa like '%" & TextBox9.Text & "%';"

                cmd = New MySqlCommand(wew, SQLConnection)
                SQLConnection.Open()
                rer = cmd.ExecuteReader
                While rer.Read
                    Dim com = rer.GetString("compa")
                    Dim cou = rer.GetString("adr")
                    Dim tin = rer.GetString("tin")
                    TextBox14.Text = tin
                    TextBox10.Text = cou
                    TextBox9.Text = com
                End While
                SQLConnection.Close()
            End If


        Catch ex As Exception
            SQLConnection.Close()
        End Try
    End Sub

    Private Sub TextBox9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox9.TextChanged
        Try
            If TextBox9.Text <> "" Then
                Dim wew As String = "SELECT * FROM sales where compa like '%" & TextBox9.Text & "%';"
                cmd = New MySqlCommand(wew, SQLConnection)
                SQLConnection.Open()
                rer = cmd.ExecuteReader
                While rer.Read
                    Dim cou = rer.GetString("adr")
                    Dim tin = rer.GetString("tin")
                    TextBox14.Text = tin
                    TextBox10.Text = cou

                End While
                SQLConnection.Close()
            End If
        Catch ex As Exception
            SQLConnection.Close()
        End Try

    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        If CheckBox3.Checked = True Then
            Dim total As Double
            For x As Integer = 0 To DataGridView2.Rows.Count - 1
                Dim amou As Double
                Dim che As Double
                If DataGridView2.Rows(x).Cells(2).Value < 0 Then
                    che = DataGridView2.Rows(x).Cells(2).Value * -1
                Else
                    che = DataGridView2.Rows(x).Cells(2).Value
                End If
                amou = che * DataGridView2.Rows(x).Cells(5).Value
                total = total + amou
                If amou = 0 Then
                    Exit For
                End If
                Label17.Text = Format(Val(total), "#,##0.00")
                Dim net As Double
                net = total / 1.12
                Label18.Text = Format(Val(net * (TextBox16.Text / 100)), "#,##0.00")
                Dim lv As Double
                lv = total - net * (TextBox16.Text / 100)
                Label19.Text = Format(Val(lv), "#,##0.00")
                If CheckBox2.Checked = True Then
                    Label18.Text = ""
                    Label19.Text = ""
                End If
            Next

        End If
        prin2()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If TextBox12.Text = "" Then
            TextBox12.Text = ComboBox1.Text
        End If
    End Sub

    
    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Label44.Text = ""
        Form6.ShowDialog()

    End Sub

    
    
    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        Form7.show

    End Sub

    Private Sub DataGridView5_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView5.CellContentClick

    End Sub
End Class
