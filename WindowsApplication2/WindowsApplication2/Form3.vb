Imports MySql.Data.MySqlClient
Public Class Form3
    'Dim serverstring As String = "Server=localhost;UserId=root;Password='admin101';Database=scancode"
    Dim serverstring As String = "Server=192.168.1.32;UserId='scancode';Password='admin101';Database=scancode"
    Dim SQLConnection As MySqlConnection = New MySqlConnection
    Dim cmd As MySqlCommand = New MySqlCommand
    Dim rer As MySqlDataReader
    Dim formattedDate As String = Date.Today.ToString("yyyy/MM/dd")
    Dim how As Integer
    Public panga As String

    Public Sub ad()
        Dim upt2 As String = "INSERT INTO hist(code, quan, name,dat,company,price,size,descri,unit,pnme) Values('" & TextBox1.Text & "','" & TextBox5.Text & "','" & panga & "','" & formattedDate & "','" & TextBox7.Text & "'," & TextBox6.Text & ",'" & TextBox4.Text & "','" & TextBox3.Text & "','" & ComboBox2.Text & "','" & TextBox2.Text & "');"
        SQLConnection.Open()
        With cmd
            .CommandText = upt2
            .Connection = SQLConnection
            .ExecuteNonQuery()
        End With
        SQLConnection.Close()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        MsgBox("New item Added!")
        Form1.loa()
    End Sub
    Public Sub ad2()
        Dim upt2 As String = "INSERT INTO items(code, name, descri, size, quan, dat, typ,price,unit) Values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "'," & TextBox5.Text & ",'" & formattedDate & "','" & ComboBox1.Text & "'," & TextBox6.Text & ",'" & ComboBox2.Text & "');"
        SQLConnection.Open()
        With cmd
            .CommandText = upt2
            .Connection = SQLConnection
            .ExecuteNonQuery()
        End With
        SQLConnection.Close()
        ad()
    End Sub
    Public Sub sear()
        Dim wew As String = "SELECT count(code) FROM items i where i.code= '" & TextBox1.Text & "';"
        cmd = New MySqlCommand(wew, SQLConnection)
        SQLConnection.Open()
        rer = cmd.ExecuteReader
        While rer.Read
            Dim cou = rer.GetInt32("count(code)")
            how = cou
        End While
        SQLConnection.Close()
            ad2()


    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" Then
            sear()
        Else
            MsgBox("Empty")
        End If
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        sear()
    End Sub

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SQLConnection.ConnectionString = serverstring
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        Try
            Dim b As Double
            If TextBox5.Text <> "" Then
                b = b + TextBox5.Text
            End If
        Catch ex As Exception
            MsgBox("Numbers Only")
            TextBox5.Text = ""
            TextBox5.Focus()
        End Try
       
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged
        Try
            If TextBox6.Text <> "" Then
                Dim a As Double
                a = a + TextBox6.Text
            End If
        Catch ex As Exception
            TextBox6.Text = ""
            MsgBox("NUMBERS only")
        End Try
        
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
       Textbox1.Text = UCase(Textbox1.Text)
    End Sub

   
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class