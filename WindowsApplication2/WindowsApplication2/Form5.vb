Imports MySql.Data.MySqlClient
Public Class Form5
    Public code As String
    Dim serverstring As String = "Server=192.168.1.32;UserId='scancode';Password='admin101';Database=scancode"
    'Dim serverstring As String = "Server=localhost;UserId=root;Password='admin101';Database=scancode"
    Dim SQLConnection As MySqlConnection = New MySqlConnection
    Dim cmd As MySqlCommand = New MySqlCommand
    Dim rer As MySqlDataReader
    Dim formattedDate As String = Date.Today.ToString("yyyy/MM/dd")
    Dim usr As String
    Dim pas As String
    Dim nme As String
    Dim ty As String
    Public Sub sear()
        Dim wew As String = "SELECT * FROM `user` u where  u.username = '" & TextBox1.Text & "';"
        cmd = New MySqlCommand(wew, SQLConnection)
        SQLConnection.Open()
        rer = cmd.ExecuteReader
        While rer.Read
            Dim nam = rer.GetString("name")
            Dim ps = rer.GetString("pass")
            Dim siz = rer.GetString("username")
            Dim typ = rer.GetString("type")
            ty = typ
            nme = nam
            pas = ps
            usr = siz
        End While
        If usr = TextBox1.Text And pas = TextBox2.Text Then
            TextBox1.Text = ""
            TextBox2.Text = ""
            Form1.panga = nme
            Form3.panga = nme
            Form1.typ = ty
            Form2.typ = ty
            Form7.typ = ty

            MsgBox("WELCOME :" & nme & "!")
            Form1.ShowDialog()
        Else
            MsgBox("Wrong Username or Password!")
        End If
        SQLConnection.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" Then
            sear()
        Else
            MsgBox("Empty Username OR Password")
        End If
    End Sub

    Private Sub Form5_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SQLConnection.ConnectionString = serverstring
        'TextBox1.Text = "bengz"
        'TextBox2.Text = "admin101"
        TextBox1.Select
    End Sub
    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = Keys.Enter Then
            If TextBox1.Text <> "" And TextBox2.Text <> "" Then
                sear()
            Else
                MsgBox("Empty Username OR Password")
            End If
        End If
    End Sub
    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If TextBox1.Text <> "" And TextBox2.Text <> "" Then
                sear()
            Else
                MsgBox("Empty Username OR Password")
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If TextBox2.PasswordChar = "*" Then
            TextBox2.PasswordChar = ""
        Else
            TextBox2.PasswordChar = "*"
        End If
    End Sub


    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim url As String = "https://scancode.com.ph/"

        Process.Start(url)
    End Sub

    
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class