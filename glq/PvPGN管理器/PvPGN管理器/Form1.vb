Imports System
Imports System.Data
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports System.Management
Imports System.ServiceProcess
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form1
    Inherits System.Windows.Forms.Form
    Dim a0 As DateTime = #6/13/2010 1:00:00 PM#
    Dim a1 As DateTime = #6/13/2010 1:00:10 PM#
    Dim outtime As TimeSpan = a1 - a0
    Dim conn As MySqlConnection
    Dim data As DataTable
    Dim da As MySqlDataAdapter
    Dim cb As MySqlCommandBuilder
    Dim d2cs_server_string As String
    Dim d2dbs_server_string As String
    Dim flag1 As String = "0"
    Dim flag2 As String = "0"
    Dim flag3 As String = "0"
    Dim flag4 As String = "0"
    Dim flag5 As String = "0"
    Dim flag6 As String = "0"
    Dim flag7 As String = "0"

    Private Sub showbutton()
        Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        '用户管理按钮
        If TextBox_acc_username.Text <> "" And Button_con_to_sql.Enabled = False And TextBox_database_name.Text <> "" Then
            Button_set_to_admin.Enabled = True
            Button_unset_to_admin.Enabled = True
            Button_set_to_op.Enabled = True
            Button_unset_to_op.Enabled = True
            Button_set_lockk.Enabled = True
            Button_unset_lockk.Enabled = True
            Button_set_mute.Enabled = True
            Button_unset_mute.Enabled = True
            Button_set_flags.Enabled = True
            'Button_del_user.Enabled = True
        Else
            Button_set_to_admin.Enabled = False
            Button_unset_to_admin.Enabled = False
            Button_set_to_op.Enabled = False
            Button_unset_to_op.Enabled = False
            Button_set_lockk.Enabled = False
            Button_unset_lockk.Enabled = False
            Button_set_mute.Enabled = False
            Button_unset_mute.Enabled = False
            Button_del_user.Enabled = False
            Button_set_flags.Enabled = False
        End If
        '用户管理按钮结束

        '数据库联接与否刷新按钮
        '已经连接数据库
        If Button_con_to_sql.Enabled = False Then
            Button_close_sql.Enabled = True
            TextBox_database_name.ReadOnly = True
            TextBox_sql_password.ReadOnly = True
            TextBox_sql_root.ReadOnly = True
            TextBox_sql_serverip.ReadOnly = True
            '是否没有创建数据库
            If TextBox_database_name.Text = "" Then
                If reg_config.GetValue("初始化数据库", "0") = "0" Then
                    Button_create_pvpgn_sql.Enabled = True
                Else
                    Button_create_pvpgn_sql.Enabled = False
                End If
                '已经连接到pvpgn
            ElseIf TextBox_database_name.Text = "pvpgn" Then
                Button_close_sql.Enabled = True
                'Button_del_pvpgn_sql.Enabled = True
                Button_bak_pvpgn_sql.Enabled = True
                Button_res_pvpgn_sql.Enabled = True
                CheckBox_timer_backup.Enabled = True
                CheckBox_timer_autolock.Enabled = True
                If reg_config Is Nothing Then
                    reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
                End If
                If reg_config IsNot Nothing Then
                    If reg_config.GetValue("添加形象功能", "0") = "0" And Button_create_pvpgn_sql.Enabled = False Then
                        Button_add_flags.Enabled = True
                    Else
                        Button_add_flags.Enabled = False
                    End If

                    If reg_config.GetValue("添加形象定时功能", "0") = "0" And Button_create_pvpgn_sql.Enabled = False Then
                        Button_add_flags_exp_date.Enabled = True
                    Else
                        Button_add_flags_exp_date.Enabled = False
                    End If

                    If reg_config.GetValue("添加锁定定时功能", "0") = "0" And Button_create_pvpgn_sql.Enabled = False Then
                        Button_add_unset_lock_exp_date.Enabled = True
                    Else
                        Button_add_unset_lock_exp_date.Enabled = False
                    End If

                    If reg_config.GetValue("添加禁言定时功能", "0") = "0" And Button_create_pvpgn_sql.Enabled = False Then
                        Button_add_unset_mute_exp_date.Enabled = True
                    Else
                        Button_add_unset_mute_exp_date.Enabled = False
                    End If
                End If
            End If



            '状态为断开连接的话
        Else
            Button_close_sql.Enabled = False
            Button_create_pvpgn_sql.Enabled = False
            'Button_del_pvpgn_sql.Enabled = False
            Button_add_flags.Enabled = False
            Button_add_flags_exp_date.Enabled = False
            Button_add_unset_lock_exp_date.Enabled = False
            Button_add_unset_mute_exp_date.Enabled = False
            Button_res_pvpgn_sql.Enabled = False
            Button_bak_pvpgn_sql.Enabled = False

            TextBox_sql_serverip.ReadOnly = False
            TextBox_sql_root.ReadOnly = False
            TextBox_sql_password.ReadOnly = False
            TextBox_database_name.ReadOnly = False

            CheckBox_timer_backup.Enabled = False
            CheckBox_timer_autolock.Enabled = False


        End If

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        If reg_config Is Nothing Then
            reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
        End If
        If reg_config IsNot Nothing Then
            reg_config.SetValue("TextBox_sql_serverip", TextBox_sql_serverip.Text)
            reg_config.SetValue("TextBox_sql_root", TextBox_sql_root.Text)
            reg_config.SetValue("TextBox_database_name.Text", TextBox_database_name.Text)
            If RadioButton_system_x64.Checked = True Then
                reg_config.SetValue("RadioButton_system_x64", "1")
            Else
                reg_config.SetValue("RadioButton_system_x64", "0")
            End If

            If RadioButton_d2_110.Checked = True Then
                reg_config.SetValue("RadioButton_d2_110", "1")
            Else
                reg_config.SetValue("RadioButton_d2_110", "0")
            End If

            reg_config.SetValue("TextBox_acc_username", TextBox_acc_username.Text)
            'reg_config.SetValue("ComboBox_flags", flag_no7.Text)

            'If CheckBox_0x20.Checked = True Then
            'reg_config.SetValue("CheckBox_0x20", "1")
            ' Else
            ' reg_config.SetValue("CheckBox_0x20", "0")
            'End If

            If CheckBox_pvpgn.Checked = True Then
                reg_config.SetValue("CheckBox_pvpgn", "1")
            Else
                reg_config.SetValue("CheckBox_pvpgn", "0")
            End If

            If CheckBox_d2cs.Checked = True Then
                reg_config.SetValue("CheckBox_d2cs", "1")
            Else
                reg_config.SetValue("CheckBox_d2cs", "0")
            End If

            If CheckBox_d2dbs.Checked = True Then
                reg_config.SetValue("CheckBox_d2dbs", "1")
            Else
                reg_config.SetValue("CheckBox_d2dbs", "0")
            End If

            If CheckBox_d2gs.Checked = True Then
                reg_config.SetValue("CheckBox_d2gs", "1")
            Else
                reg_config.SetValue("CheckBox_d2gs", "0")
            End If

            If CheckBox_timer_backup.Checked = True Then
                reg_config.SetValue("CheckBox_timer_backup", "1")
            Else
                reg_config.SetValue("CheckBox_timer_backup", "0")
            End If

            If CheckBox_timer_stop_pvpgn.Checked = True Then
                reg_config.SetValue("CheckBox_timer_stop_pvpgn", "1")
            Else
                reg_config.SetValue("CheckBox_timer_stop_pvpgn", "0")
            End If

            If CheckBox_timer_re_pvpgn.Checked = True Then
                reg_config.SetValue("CheckBox_timer_re_pvpgn", "1")
            Else
                reg_config.SetValue("CheckBox_timer_re_pvpgn", "0")
            End If

            If CheckBox_re_jisuanji.Checked = True Then
                reg_config.SetValue("CheckBox_re_jisuanji", "1")
            Else
                reg_config.SetValue("CheckBox_re_jisuanji", "0")
            End If

            If CheckBox_timer_autolock.Checked = True Then
                reg_config.SetValue("CheckBox_timer_autolock", "1")
            Else
                reg_config.SetValue("CheckBox_timer_autolock", "0")
            End If

            If CheckBox_save_password.Checked = True Then
                reg_config.SetValue("CheckBox_save_password", "1")
                reg_config.SetValue("TextBox_sql_password", TextBox_sql_password.Text)
            Else
                reg_config.SetValue("CheckBox_save_password", "0")
            End If

            reg_config.SetValue("TextBox_d2_path", TextBox_d2_path.Text)
            reg_config.SetValue("TextBox_sqlbak_name", TextBox_sqlbak_name.Text)
            reg_config.SetValue("ComboBox_backup_h", ComboBox_backup_h.Text)
            reg_config.SetValue("ComboBox_backup_m", ComboBox_backup_m.Text)
            reg_config.SetValue("ComboBox_stop_pvpgn_houre", ComboBox_stop_pvpgn_houre.Text)
            reg_config.SetValue("ComboBox_stop_pvpgn_m", ComboBox_stop_pvpgn_m.Text)
            reg_config.SetValue("ComboBox_re_pvpgn_houre", ComboBox_re_pvpgn_houre.Text)
            reg_config.SetValue("ComboBox_re_pvpgn_m", ComboBox_re_pvpgn_m.Text)
            reg_config.SetValue("ComboBox_auto_lock_houre", ComboBox_auto_lock_houre.Text)
            reg_config.SetValue("ComboBox_auto_lock_m", ComboBox_auto_lock_m.Text)
            reg_config.SetValue("TextBox_auto_lock_day", TextBox_auto_lock_day.Text)
            'regVersion.SetValue("Version", intVersion)
        End If
        reg_config.Close()
    End Sub



    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim sspvpgn As New ServiceController("pvpgn")
        'Dim ssd2cs As New ServiceController("d2cs")
        'Dim ssd2dbs As New ServiceController("d2dbs")
        'Dim pvpgnname As String
        'Dim d2csname As String
        'Dim d2dbsname As String
        'Dim d2gsname As String
        'pvpgnname = sspvpgn.ServiceName.ToString
        'd2csname = ssd2cs.ServiceName.ToString
        'd2dbsname = ssd2dbs.ServiceName.ToString
        'd2gsname = ssd2gs.ServiceName.ToString
        'If pvpgnname = "" Then
        'Else
        'Select Case sspvpgn.Status
        '    Case ServiceControllerStatus.Running
        'Label15.Text = "正在运行"
        '   Case ServiceControllerStatus.Stopped
        'Label15.Text = "已停止"
        'End Select
        'End If

        '        If d2csname = "" Then
        'Select Case ssd2cs.Status
        '   Case ServiceControllerStatus.Running
        'Label16.Text = "正在运行"
        '    Case ServiceControllerStatus.Stopped
        'Label16.Text = "已停止"
        'End Select
        'End If

        '        If d2dbsname = "" Then
        'Select Case ssd2dbs.Status
        '   Case ServiceControllerStatus.Running
        'Label17.Text = "正在运行"
        '   Case ServiceControllerStatus.Stopped
        'Label17.Text = "已停止"
        'End Select
        '     End If

        If DateString > "2016-3-30" Then
            Close()
        End If
        load_config()
        d2gsver()
        shuaxin()
        '自动连接数据库
        If CheckBox_save_password.Checked = True Then
            If Not conn Is Nothing Then conn.Close()
            Dim connStr As String
            connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        TextBox_sql_serverip.Text, TextBox_sql_root.Text, TextBox_sql_password.Text, TextBox_database_name.Text)
            Try
                conn = New MySqlConnection(connStr)
                conn.Open()
                Button_con_to_sql.Enabled = False
                '刷新各种按钮状态
                showbutton()
                'GetDatabases()
                'Catch ex As MySqlException
                '
            Catch ex As MySql.Data.MySqlClient.MySqlException
                'Select Case ex.Number
                ' Case 0
                ' MessageBox.Show("账号密码不对")
                ' Case 1042
                '  MessageBox.Show("找不到服务器")

                ' End Select
                'MessageBox.Show(ex.Number)
                'MessageBox.Show(ex.Message)
            End Try
        End If
        '自动连接数据库结束

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_con_to_sql.Click
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
    TextBox_sql_serverip.Text, TextBox_sql_root.Text, TextBox_sql_password.Text, TextBox_database_name.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            Button_con_to_sql.Enabled = False
            'GetDatabases()
            'Catch ex As MySqlException
            '
        Catch ex As MySql.Data.MySqlClient.MySqlException
            Select Case ex.Number
                Case 0
                    MessageBox.Show("账号密码不对")
                Case 1042
                    MessageBox.Show("找不到服务器")

            End Select
            'MessageBox.Show(ex.Number)
            'MessageBox.Show(ex.Message)
        End Try
    End Sub





    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_del_user.Click
        'Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 1000", conn)
        'Dim deluserstr As String
        'deluserstr = String.Format("DELETE FROM `pvpgn_bnet` WHERE (`username`='{0}') LIMIT 1", username.Text)
        'Dim deluser As New MySqlCommand(deluserstr, conn)
        'selectpvpgn.ExecuteNonQuery()
        'deluser.ExecuteNonQuery()

    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("mailto://yjfyy@163.com")
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start("http://hi.baidu.com/yjfyy")
    End Sub

    Private Sub TabPage4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage4.Click

    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        'MessageBox.Show("安装开始后会出现CMD窗口，当显示""Installing Service""后关掉CMD窗口")
        Shell("PvPGNConsole.exe -s install", vbHide)
        'MessageBox.Show(i)
        MessageBox.Show("PvPGN已安装")
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        Dim sspvpgn As New ServiceController("pvpgn")
        If sspvpgn.Status.Equals(ServiceControllerStatus.Running) Then
            MessageBox.Show("请停止PvPGN后重试")
        Else
            Shell("PvPGNConsole.exe -s uninstall", vbHide)
            MessageBox.Show("卸载完成")
        End If
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim sspvpgn As New ServiceController("pvpgn")
        sspvpgn.Refresh()
        MessageBox.Show("已重启")
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ssd2cs As New ServiceController("d2cs")
        ssd2cs.Refresh()
        MessageBox.Show("已重启")
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ssd2dbs As New ServiceController("d2dbs")
        ssd2dbs.Refresh()
        MessageBox.Show("已重启")
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim sspvpgn As New ServiceController("pvpgn")
        Dim ssd2cs As New ServiceController("d2cs")
        Dim ssd2dbs As New ServiceController("d2dbs")
        sspvpgn.Refresh()
        ssd2cs.Refresh()
        ssd2dbs.Refresh()
        MessageBox.Show("已全部重启")
    End Sub




    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        Shell("explorer.exe .\conf\", AppWinStyle.MaximizedFocus)
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Shell(".\d2gs\d2gs.reg", vbHide)

    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub Button24_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click
        d2gsdefconf()
        Shell(".\d2gs\d2gssvc.exe -i", vbHide)
        MsgBox("安装完成")
    End Sub

    Private Sub Button26_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        Dim ssd2gs As New ServiceController("d2gs")
        If ssd2gs.Status.Equals(ServiceControllerStatus.Running) Then
            MessageBox.Show("请停止D2GS服务后重试")
        Else
            My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\D2Server\D2GS")
            My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\D2Server")
            Shell(".\d2gs\d2gssvc.exe -u", vbHide)
            MessageBox.Show("卸载完成")
        End If
    End Sub


    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button31.Click
        Button31.Text = "正在复制"
        Button31.Enabled = False
        ProgressBar1.Visible = True
        ProgressBar1.Value = 0
        Dim i
        Dim d2gsneedfiles(3) As String
        d2gsneedfiles(0) = "d2data.mpq"
        d2gsneedfiles(1) = "d2exp.mpq"
        d2gsneedfiles(2) = "d2speech.mpq"
        d2gsneedfiles(3) = "d2sfx.mpq"
        For i = 0 To 3
            If System.IO.File.Exists(TextBox_d2_path.Text + "\" + d2gsneedfiles(i)) Then
                ProgressBar1.Value = ProgressBar1.Value + 20
                System.IO.File.Copy(TextBox_d2_path.Text + "\" + d2gsneedfiles(i), ".\d2gs\" + d2gsneedfiles(i), True)
            Else : MsgBox(d2gsneedfiles(i) + "没有找到")
            End If
        Next
        ProgressBar1.Value = 100
        ProgressBar1.Visible = False
        Button31.Text = "复制所需文件"
        MsgBox("复制完成")
        Button31.Enabled = True
    End Sub






    Private Sub d2gsdefconf()
        Dim d2gsregname As String = "HKEY_LOCAL_MACHINE\SOFTWARE\D2Server\D2GS"
        Microsoft.Win32.Registry.SetValue(d2gsregname, "", """Diablo II Close Game Server""")
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2CSPort", 6113, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2DBSPort", 6114, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2CSSecrect", "")
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxPreferUsers", 0, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxGameLife", 31372, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AdminPassword", "9e75a42100e1b9e0b5d3873045084fae699adcb0")
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AdminPort", 8888, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AdminTimeout", 3600, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "EnableNTMode", 1, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "EnablePreCacheMode", 1, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "IdleSleep", 1, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "BusySleep", 1, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "CharPendingTimeout", 600, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "EnableGELog", 1, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "DebugNetPacket", 0, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "DebugEventCallback", 0, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "EnableGEMsg", 0, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "IntervalReconnectD2CS", 50, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "EnableGEPatch", 1, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AutoUpdate", 1, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "GSShutdownInterval", 15, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MultiCPUMask", 1, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "EnableGSLog", 0, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MOTD", "")
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AutoUpdateUrl", "")
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AutoUpdateVer", 0, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AutoUpdateTimeout", 3600, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxPacketPerSecond", 1200, Microsoft.Win32.RegistryValueKind.DWord)
    End Sub

    Private Sub Button32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button32.Click
        'Dim maxgames As Integer
        'maxgames = CInt(TextBox10.Text)
        Dim d2gsregname As String = "HKEY_LOCAL_MACHINE\SOFTWARE\D2Server\D2GS"
        Dim gs_telnet_password_hash As String
        Shell("cmd /c bnhash.exe " & TextBox_d2gsconfig_telnet_password.Text & " >temp.txt", AppWinStyle.Hide)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2CSIP", TextBox_d2gsconfig_d2csip.Text)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2DBSIP", TextBox_d2gsconfig_d2dbsip.Text)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxGames", TextBox_d2gsconfig_maxgame.Text, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxGameLife", TextBox_d2gsconfig_MaxGameLife.Text)
        MsgBox("GS Telnet 密码修改后需重启D2GS才能生效")
        gs_telnet_password_hash = My.Computer.FileSystem.ReadAllText("temp.txt")
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AdminPassword", gs_telnet_password_hash)
        MsgBox("设置成功")
        My.Computer.FileSystem.DeleteFile("temp.txt")
    End Sub



    Private Sub Button35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_res_pvpgn_sql.Click
        If RadioButton_system_x86.Checked = True Then
            Try
                Shell("cmd /c ..\mysql_x86.exe --host=" + TextBox_sql_serverip.Text + " --user=" + TextBox_sql_root.Text + " --password=" + TextBox_sql_password.Text + " < " + TextBox_sqlbak_name.Text, AppWinStyle.Hide)
                MessageBox.Show("还原数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Else
            Try
                Shell("cmd /c mysql_x64.exe --host=" + TextBox_sql_serverip.Text + " --user=" + TextBox_sql_root.Text + " --password=" + TextBox_sql_password.Text + " < " + TextBox_sqlbak_name.Text, AppWinStyle.Hide)
                MessageBox.Show("还原数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button36.Click
        Shell(d2cs_server_string + "Console.exe -s install", vbHide)
        'MessageBox.Show(i)
        MsgBox("D2CS已安装")
    End Sub

    Private Sub Button37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button37.Click
        Shell(d2dbs_server_string + "Console.exe -s install", vbHide)
        'MessageBox.Show(i)
        MsgBox("D2DBS已安装")
    End Sub

    Private Sub Button38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button38.Click
        Dim ssd2cs As New ServiceController(d2cs_server_string)
        If ssd2cs.Status.Equals(ServiceControllerStatus.Running) Then
            MessageBox.Show("请停止D2CS服务后重试")
        Else
            Shell("d2csConsole.exe -s uninstall", vbHide)
            'MessageBox.Show(i)
            MessageBox.Show("卸载完成")
        End If
    End Sub

    Private Sub Button39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button39.Click
        Dim ssd2dbs As New ServiceController(d2dbs_server_string)

        If ssd2dbs.Status.Equals(ServiceControllerStatus.Running) Then
            MessageBox.Show("请停止D2DBS服务后重试")
        Else
            Shell("d2dbsConsole.exe -s uninstall", vbHide)
            'MessageBox.Show(i)
            MessageBox.Show("卸载完成")
        End If
    End Sub


    Public Sub stop_pvpgn_server()
        Dim sspvpgn As New ServiceController("pvpgn")
        Try
            If sspvpgn.Status <> ServiceControllerStatus.Stopped Then
                sspvpgn.Stop()
                sspvpgn.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
            End If
        Catch ex As Exception
            MsgBox("PvPGN未能停止，请重试，多次重试仍然不行请重启计算机")
        End Try
    End Sub
    Public Sub stop_mysql_server()
        Dim ssmysql As New ServiceController("mysql")
        Try
            If ssmysql.Status <> ServiceControllerStatus.Stopped Then
                ssmysql.Stop()
                ssmysql.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
            End If
        Catch ex As Exception
            MsgBox("MySQL未能停止，请重试，多次重试仍然不行请重启计算机")
        End Try
    End Sub

    Public Sub run_pvpgn_server()
        Dim sspvpgn As New ServiceController("pvpgn")
        Try
            sspvpgn.Start()
            sspvpgn.WaitForStatus(ServiceControllerStatus.Running, outtime)
        Catch When sspvpgn.Status = ServiceControllerStatus.Running
            MessageBox.Show("PvPGN正在运行，不能重复启动")
            Exit Sub
        Catch ex As Exception
            MessageBox.Show("不能启动PvPGN,请检查bnet.conf各项配置")
            Exit Sub
        End Try
    End Sub

    Public Sub run_mysql_server()
        Dim rsmysql As New ServiceController("MySQL")
        Try
            rsmysql.Start()
            rsmysql.WaitForStatus(ServiceControllerStatus.Running, outtime)
        Catch When rsmysql.Status = ServiceControllerStatus.Running
            MessageBox.Show("MySQL正在运行，不要重复启动。")
            Exit Sub
        Catch ex As Exception
            MessageBox.Show("不能启动MySQL服务，请重试。")
            Exit Sub
        End Try
    End Sub

    Public Sub stop_d2cs_server()
        Dim ssd2cs As New ServiceController(d2cs_server_string)
        Try
            If ssd2cs.Status <> ServiceControllerStatus.Stopped Then

                ssd2cs.Stop()
                ssd2cs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
            End If
        Catch ex As Exception
            MsgBox("D2CS未能停止，请重试，多次重试仍然不行请重启计算机")
        End Try
    End Sub

    Public Sub run_d2cs_server()
        Dim ssd2cs As New ServiceController(d2cs_server_string)
        Try
            ssd2cs.Start()
            ssd2cs.WaitForStatus(ServiceControllerStatus.Running, outtime)
        Catch When ssd2cs.Status = ServiceControllerStatus.Running
            MessageBox.Show("D2CS正在运行，不能重复启动")
            Exit Sub
        Catch ex As Exception
            MessageBox.Show("不能启动D2CS,请检查d2cs.conf各项配置")
            Exit Sub
        End Try
    End Sub

    Public Sub stop_d2dbs_server()
        Dim ssd2dbs As New ServiceController(d2dbs_server_string)
        Try
            If ssd2dbs.Status <> ServiceControllerStatus.Stopped Then
                ssd2dbs.Stop()
                ssd2dbs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
            End If
        Catch ex As Exception
            MsgBox("D2DBS未能停止，请重试，多次重试仍然不行请重启计算机")
        End Try
    End Sub

    Public Sub run_d2dbs_server()
        Dim ssd2dbs As New ServiceController(d2dbs_server_string)
        Try
            ssd2dbs.Start()
            ssd2dbs.WaitForStatus(ServiceControllerStatus.Running, outtime)
        Catch When ssd2dbs.Status = ServiceControllerStatus.Running
            MessageBox.Show("D2DBS正在运行，不能重复启动")
            Exit Sub
        Catch ex As Exception
            MessageBox.Show("不能启动D2DBS,请检查d2dbs.conf各项配置")
            Exit Sub
        End Try
    End Sub

    Public Sub stop_d2gs_server()
        Dim ssd2gs As New ServiceController("d2gs")
        Try
            If ssd2gs.Status <> ServiceControllerStatus.Stopped Then
                ssd2gs.Stop()
                ssd2gs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
            End If
        Catch ex As Exception
            MsgBox("D2GS未能停止，请重试，多次重试仍然不行请重启计算机")
        End Try
    End Sub

    Public Sub run_d2gs_server()
        Dim ssd2gs As New ServiceController("d2gs")
        Try
            ssd2gs.Start()
            ssd2gs.WaitForStatus(ServiceControllerStatus.Running, outtime)
        Catch When ssd2gs.Status = ServiceControllerStatus.Running
            MessageBox.Show("D2GS正在运行，不能重复启动")
            Exit Sub
        Catch ex As Exception
            MessageBox.Show("不能启动D2GS,请检查D2GS配置")
            Exit Sub
        End Try
    End Sub

    Private Sub Button_restart_pvpgn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_restart_pvpgn.Click
        stop_pvpgn_server()
        run_pvpgn_server()
        MessageBox.Show("重启指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_restart_d2cs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_restart_d2cs.Click
        stop_d2cs_server()
        run_d2cs_server()
        MessageBox.Show("重启指令执行完毕。")
        shuaxin()
    End Sub


    Private Sub Button_restart_d2dbs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_restart_d2dbs.Click
        stop_d2dbs_server()
        run_d2dbs_server()
        MessageBox.Show("重启指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_restart_d2gs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_restart_d2gs.Click
        stop_d2gs_server()
        run_d2gs_server()
        MessageBox.Show("重启指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_stop_pvpgn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_stop_pvpgn.Click
        stop_pvpgn_server()
        MessageBox.Show("停止指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_stop_d2cs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_stop_d2cs.Click
        stop_d2cs_server()
        MessageBox.Show("停止指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_stop_d2dbs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_stop_d2dbs.Click
        stop_d2dbs_server()
        MessageBox.Show("停止指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_stop_d2gs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_stop_d2gs.Click
        stop_d2gs_server()
        MessageBox.Show("停止指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_stop_select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_stop_select.Click
        If CheckBox_pvpgn.Checked Then
            stop_pvpgn_server()
        End If

        If CheckBox_d2cs.Checked Then
            stop_d2cs_server()
        End If

        If CheckBox_d2dbs.Checked Then
            stop_d2dbs_server()
        End If

        If CheckBox_d2gs.Checked Then
            stop_d2gs_server()
        End If

        MsgBox("停止指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_restart_select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_restart_select.Click
        If CheckBox_pvpgn.Checked = True Then
            stop_pvpgn_server()
            run_pvpgn_server()
        End If

        If CheckBox_d2cs.Checked = True Then
            stop_d2cs_server()
            run_d2cs_server()
        End If

        If CheckBox_d2dbs.Checked = True Then
            stop_d2dbs_server()
            run_d2dbs_server()
        End If

        If CheckBox_d2gs.Checked = True Then
            stop_d2gs_server()
            run_d2gs_server()
        End If
        MessageBox.Show("重启指令执行完毕。")
        shuaxin()
    End Sub

    Private Sub Button_refurbish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_refurbish.Click
        shuaxin()
    End Sub


    Private Sub Button_set_to_admin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_set_to_admin.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 30000", conn)
        Dim setadminstr As String
        Dim setcommandgroupsstr As String
        Dim setflagsstr As String
        setadminstr = String.Format("UPDATE `pvpgn_bnet` SET `auth_admin`='true' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        setcommandgroupsstr = String.Format("UPDATE `pvpgn_bnet` SET `auth_command_groups`='255' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        setflagsstr = String.Format("UPDATE `pvpgn_bnet` SET `flags_initial`='1' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        Dim setadmin As New MySqlCommand(setadminstr, conn)
        Dim setcommandgroups As New MySqlCommand(setcommandgroupsstr, conn)
        Dim setflags As New MySqlCommand(setflagsstr, conn)
        selectpvpgn.ExecuteNonQuery()
        setadmin.ExecuteNonQuery()
        setcommandgroups.ExecuteNonQuery()
        setflags.ExecuteNonQuery()
        MsgBox("设置成功")
    End Sub

    Private Sub Button_unset_to_admin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_unset_to_admin.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
        Dim unsetadminstr As String
        Dim setcommandgroupsstr As String
        Dim setflagsstr As String
        unsetadminstr = String.Format("UPDATE `pvpgn_bnet` SET `auth_admin`='false' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        setcommandgroupsstr = String.Format("UPDATE `pvpgn_bnet` SET `auth_command_groups`='1' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        setflagsstr = String.Format("UPDATE `pvpgn_bnet` SET `flags_initial`='0' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        Dim unsetadmin As New MySqlCommand(unsetadminstr, conn)
        Dim setcommandgroups As New MySqlCommand(setcommandgroupsstr, conn)
        Dim setflags As New MySqlCommand(setflagsstr, conn)
        selectpvpgn.ExecuteNonQuery()
        unsetadmin.ExecuteNonQuery()
        setcommandgroups.ExecuteNonQuery()
        setflags.ExecuteNonQuery()
        MsgBox("设置成功")
    End Sub

    Private Sub Button_path_bnetdsql_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_add_flags.Click
        'Dim pathbnet As New MySqlCommand("ALTER TABLE `pvpgn_bnet` ADD COLUMN `flags_initial`  int(11) NULL;", conn)
        'pathbnet.ExecuteNonQuery()
        'MsgBox("数据库已修正，可以修改用户频道形象了")

        'Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        'Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        'If reg_config Is Nothing Then
        '    reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
        'End If
        'If reg_config IsNot Nothing Then
        '    reg_config.SetValue("添加形象功能", "1")
        'End If
        'reg_config.Close()
        'showbutton()
    End Sub

    Private Sub Button_create_pvpgn_sql_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_create_pvpgn_sql.Click
        Shell("D:\pvpgn\mysql\remysql.bat", AppWinStyle.NormalFocus, True)
        MsgBox("数据库重置完毕。")

        'Dim createpvpgnstr As String
        'createpvpgnstr = String.Format("create database pvpgn")
        'Dim createpvpgn As New MySqlCommand(createpvpgnstr, conn)
        'Try
        '    createpvpgn.ExecuteNonQuery()
        'Catch ex As MySql.Data.MySqlClient.MySqlException
        '    Select Case ex.Number
        '        Case 1007
        '            MessageBox.Show("请勿重复初始化数据库！")
        '            Exit Sub
        '    End Select
        '    'MessageBox.Show(ex.Number)
        '    'essageBox.Show(ex.Message)
        'End Try
        'MsgBox("数据库初始化成功！")
        'Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        'Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        'If reg_config Is Nothing Then
        '    reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
        'End If
        'If reg_config IsNot Nothing Then
        '    reg_config.SetValue("初始化数据库", "1")
        'End If
        'reg_config.Close()
    End Sub

    Private Sub Button_del_pvpgn_sql_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim deletepvpgnstr As String
        'deletepvpgnstr = String.Format("drop database pvpgn")
        'Dim deletepvpgn As New MySqlCommand(deletepvpgnstr, conn)
        'Try
        '    deletepvpgn.ExecuteNonQuery()
        'Catch ex As MySql.Data.MySqlClient.MySqlException
        '    MessageBox.Show(ex.Number)
        '    MessageBox.Show(ex.Message)
        '    Exit Sub
        'End Try
        'MsgBox("数据库已清除！")
        'Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        'Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        'If reg_config Is Nothing Then
        '    reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
        'End If
        'If reg_config IsNot Nothing Then
        '    reg_config.SetValue("初始化数据库", "0")
        'End If
        'reg_config.Close()
    End Sub


    Private Sub username_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_acc_username.TextChanged
        showbutton()
    End Sub


    Private Sub Button_set_flags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_set_flags.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 30000", conn)
        Dim set_flags_str As String
        Dim set_flags_exp_date_str As String

        '计算出形象代码
        If CheckBox_guanghuan.Checked = True Then
            flag6 = "2"
        Else
            flag6 = "0"
        End If
        flag_no.Text = flag5 + flag6 + flag7

        '转换flag_no.text字符为数字，再视作16进制转换为10进制
        Dim flagno = (Str("&H" & Val(flag_no.Text)))

        set_flags_str = String.Format("UPDATE `pvpgn_bnet` SET `flags_initial`='{0}' WHERE (`username`='{1}') LIMIT 1", flagno, TextBox_acc_username.Text)
        set_flags_exp_date_str = String.Format("UPDATE `pvpgn_bnet` SET `flags_exp_date`='{0}' WHERE (`username`='{1}') LIMIT 1", DateTimePicker_xingxiang.Value, TextBox_acc_username.Text)
        Dim set_flags As New MySqlCommand(set_flags_str, conn)
        Dim set_flags_exp_date As New MySqlCommand(set_flags_exp_date_str, conn)
        selectpvpgn.ExecuteNonQuery()
        Try
            set_flags.ExecuteNonQuery()
            set_flags_exp_date.ExecuteNonQuery()
            MsgBox("设置成功，" + TextBox_acc_username.Text + "的形象将于" + DateTimePicker_xingxiang.Value.Date + "失效")
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MessageBox.Show(ex.Number)
            MessageBox.Show(ex.Message)
            MsgBox("设置失败，请确认已修正数据库、用户名填写正确")
        End Try


    End Sub
    Private Sub shuaxin()
        Dim sspvpgn As New ServiceController("pvpgn")
        Dim ssd2cs As New ServiceController(d2cs_server_string)
        Dim ssd2dbs As New ServiceController(d2dbs_server_string)
        Dim ssd2gs As New ServiceController("d2gs")
        Try
            Select Case sspvpgn.Status
                Case ServiceControllerStatus.Running
                    Label15.Text = "正在运行"
                Case ServiceControllerStatus.Stopped
                    Label15.Text = "已停止"
            End Select
        Catch ex As Exception

        End Try

        Try
            Select Case ssd2cs.Status
                Case ServiceControllerStatus.Running
                    Label16.Text = "正在运行"
                Case ServiceControllerStatus.Stopped
                    Label16.Text = "已停止"
            End Select
        Catch ex As Exception

        End Try

        Try
            Select Case ssd2dbs.Status
                Case ServiceControllerStatus.Running
                    Label17.Text = "正在运行"
                Case ServiceControllerStatus.Stopped
                    Label17.Text = "已停止"
            End Select
        Catch ex As Exception

        End Try

        Try
            Select Case ssd2gs.Status
                Case ServiceControllerStatus.Running
                    Label18.Text = "正在运行"
                Case ServiceControllerStatus.Stopped
                    Label18.Text = "已停止"
            End Select
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button_set_to_op_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_set_to_op.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
        Dim set_op_str As String
        Dim set_commandgroups_str As String
        Dim set_flags_str As String
        set_op_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_operator`='true' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        set_commandgroups_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_command_groups`='6' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        set_flags_str = String.Format("UPDATE `pvpgn_bnet` SET `flags_initial`='2' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        Dim set_admin As New MySqlCommand(set_op_str, conn)
        Dim set_commandgroups As New MySqlCommand(set_commandgroups_str, conn)
        Dim set_flags As New MySqlCommand(set_flags_str, conn)
        selectpvpgn.ExecuteNonQuery()
        set_admin.ExecuteNonQuery()
        set_commandgroups.ExecuteNonQuery()
        set_flags.ExecuteNonQuery()
        MsgBox("设置成功")
    End Sub

    Private Sub Button_unset_to_op_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_unset_to_op.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
        Dim unset_op_str As String
        Dim set_commandgroups_str As String
        Dim set_flags_str As String
        unset_op_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_operator`='false' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        set_commandgroups_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_command_groups`='1' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        set_flags_str = String.Format("UPDATE `pvpgn_bnet` SET `flags_initial`='0' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        Dim unset_op As New MySqlCommand(unset_op_str, conn)
        Dim set_commandgroups As New MySqlCommand(set_commandgroups_str, conn)
        Dim set_flags As New MySqlCommand(set_flags_str, conn)
        selectpvpgn.ExecuteNonQuery()
        unset_op.ExecuteNonQuery()
        set_commandgroups.ExecuteNonQuery()
        set_flags.ExecuteNonQuery()
        MsgBox("设置成功")
    End Sub


    Private Sub Button_lockk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_set_lockk.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
        Dim set_lockk_str As String
        Dim set_lockk_exp_date_str As String
        set_lockk_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_lockk`='1' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        set_lockk_exp_date_str = String.Format("UPDATE `pvpgn_bnet` SET `lockk_exp_date`='{0}' WHERE (`username`='{1}') LIMIT 1", DateTimePicker_suoding.Value, TextBox_acc_username.Text)
        Dim set_lockk As New MySqlCommand(set_lockk_str, conn)
        Dim set_lockk_exp_date As New MySqlCommand(set_lockk_exp_date_str, conn)
        selectpvpgn.ExecuteNonQuery()
        Try
            set_lockk.ExecuteNonQuery()
            set_lockk_exp_date.ExecuteNonQuery()
            MsgBox("设置成功，" + TextBox_acc_username.Text + "将于" + DateTimePicker_suoding.Value.Date + "解除锁定")
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MessageBox.Show(ex.Number)
            MessageBox.Show(ex.Message)
            MsgBox("设置失败，请确认已修正数据库、用户名填写正确")
        End Try

    End Sub

    Private Sub Button_unlockk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_unset_lockk.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
        Dim set_unlockk_str As String
        set_unlockk_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_lockk`='0' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        Dim set_unlockk As New MySqlCommand(set_unlockk_str, conn)
        selectpvpgn.ExecuteNonQuery()
        set_unlockk.ExecuteNonQuery()
        MsgBox("设置成功")
    End Sub

    Private Sub Button_mute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_set_mute.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
        Dim set_mute_str As String
        Dim set_mute_exp_date_str As String
        set_mute_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_mute`='1' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        set_mute_exp_date_str = String.Format("UPDATE `pvpgn_bnet` SET `mute_exp_date`='{0}' WHERE (`username`='{1}') LIMIT 1", DateTimePicker_jinyan.Value, TextBox_acc_username.Text)
        Dim set_mute As New MySqlCommand(set_mute_str, conn)
        Dim set_mute_exp_date As New MySqlCommand(set_mute_exp_date_str, conn)
        selectpvpgn.ExecuteNonQuery()
        Try
            set_mute.ExecuteNonQuery()
            set_mute_exp_date.ExecuteNonQuery()
            MsgBox("设置成功，" + TextBox_acc_username.Text + "将于" + DateTimePicker_jinyan.Value.Date + "解除禁言")
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MessageBox.Show(ex.Number)
            MessageBox.Show(ex.Message)
            MsgBox("设置失败，请确认已修正数据库、用户名填写正确")
        End Try


    End Sub

    Private Sub Button_unmute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_unset_mute.Click
        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
        Dim set_unmute_str As String
        set_unmute_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_mute`='0' WHERE (`username`='{0}') LIMIT 1", TextBox_acc_username.Text)
        Dim set_unmute As New MySqlCommand(set_unmute_str, conn)
        selectpvpgn.ExecuteNonQuery()
        set_unmute.ExecuteNonQuery()
        MsgBox("设置成功")
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        TextBox_d2_path.Text = FolderBrowserDialog1.SelectedPath

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_close_sql.Click
        conn.Close()
        Button_con_to_sql.Enabled = True
        showbutton()
    End Sub


    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        OpenFileDialog1.ShowDialog()
        TextBox_sqlbak_name.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub d2gsver()
        If RadioButton_d2_109.Checked = True Then
            d2cs_server_string = "d2cs109"
            d2dbs_server_string = "d2dbs109"
        Else
            d2cs_server_string = "d2cs"
            d2dbs_server_string = "d2dbs"
        End If
    End Sub

    Private Sub RadioButton_d2_110_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_d2_110.CheckedChanged
        d2gsver()
    End Sub

    Private Sub Button18_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        TabPage3.Enabled = False
    End Sub

    Private Sub load_config()
        Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        If reg_config Is Nothing Then
            reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
        End If
        If reg_config IsNot Nothing Then
            TextBox_sql_serverip.Text = reg_config.GetValue("TextBox_sql_serverip", "127.0.0.1")
            TextBox_sql_root.Text = reg_config.GetValue("TextBox_sql_root", "root")
            TextBox_database_name.Text = reg_config.GetValue("TextBox_database_name.Text", "pvpgn")
            If reg_config.GetValue("RadioButton_system_x64", "1") = "1" Then
                RadioButton_system_x64.Checked = True
            Else
                RadioButton_system_x86.Checked = True
            End If
            If reg_config.GetValue("RadioButton_d2_110", "1") = "1" Then
                RadioButton_d2_110.Checked = True
            End If
            TextBox_acc_username.Text = reg_config.GetValue("Textbox_acc_username", "")

            'flag_no7.Text = reg_config.GetValue("ComboBox_flags", "0x0 职业形象")
            'If reg_config.GetValue("CheckBox_0x20", "1") = "1" Then
            'CheckBox_0x20.Checked = True
            'Else
            'CheckBox_0x20.Checked = False
            'End If

            If reg_config.GetValue("CheckBox_pvpgn", "1") = "1" Then
                CheckBox_pvpgn.Checked = True
            Else
                CheckBox_pvpgn.Checked = False
            End If

            If reg_config.GetValue("CheckBox_d2cs", "1") = "1" Then
                CheckBox_d2cs.Checked = True
            Else
                CheckBox_d2cs.Checked = False
            End If

            If reg_config.GetValue("CheckBox_d2dbs", "1") = "1" Then
                CheckBox_d2dbs.Checked = True
            Else
                CheckBox_d2dbs.Checked = False
            End If

            If reg_config.GetValue("CheckBox_d2gs", "1") = "1" Then
                CheckBox_d2gs.Checked = True
            Else
                CheckBox_d2gs.Checked = False
            End If

            If reg_config.GetValue("CheckBox_timer_backup", "0") = "1" Then
                CheckBox_timer_backup.Checked = True
            Else
                CheckBox_timer_backup.Checked = False
            End If

            If reg_config.GetValue("CheckBox_timer_stop_pvpgn", "0") = "1" Then
                CheckBox_timer_stop_pvpgn.Checked = True
            Else
                CheckBox_timer_stop_pvpgn.Checked = False
            End If

            If reg_config.GetValue("CheckBox_re_jisuanji", "0") = "1" Then
                CheckBox_re_jisuanji.Checked = True
            Else
                CheckBox_re_jisuanji.Checked = False
            End If

            If reg_config.GetValue("CheckBox_timer_re_pvpgn", "0") = "1" Then
                CheckBox_timer_re_pvpgn.Checked = True
            Else
                CheckBox_timer_re_pvpgn.Checked = False
            End If

            If reg_config.GetValue("CheckBox_timer_autolock", "0") = "1" Then
                CheckBox_timer_autolock.Checked = True
            Else
                CheckBox_timer_autolock.Checked = False
            End If

            If reg_config.GetValue("CheckBox_save_password", "0") = "1" Then
                CheckBox_save_password.Checked = True
                TextBox_sql_password.Text = reg_config.GetValue("TextBox_sql_password", "")
            Else
                CheckBox_save_password.Checked = False
            End If

            TextBox_d2_path.Text = reg_config.GetValue("TextBox_d2_path", "")
            TextBox_sqlbak_name.Text = reg_config.GetValue("TextBox_sqlbak_name", "")
            ComboBox_backup_h.Text = reg_config.GetValue("ComboBox_backup_h", "4")
            ComboBox_backup_m.Text = reg_config.GetValue("ComboBox_backup_m", "5")
            ComboBox_stop_pvpgn_houre.Text = reg_config.GetValue("ComboBox_stop_pvpgn_houre", "4")
            ComboBox_stop_pvpgn_m.Text = reg_config.GetValue("ComboBox_stop_pvpgn_m", "0")
            ComboBox_re_pvpgn_houre.Text = reg_config.GetValue("ComboBox_re_pvpgn_houre", "4")
            ComboBox_re_pvpgn_m.Text = reg_config.GetValue("ComboBox_re_pvpgn_m", "15")
            ComboBox_auto_lock_houre.Text = reg_config.GetValue("ComboBox_auto_lock_houre", "4")
            ComboBox_auto_lock_m.Text = reg_config.GetValue("ComboBox_auto_lock_m", "10")
            TextBox_auto_lock_day.Text = reg_config.GetValue("TextBox_auto_lock_day", "30")
            'regVersion.SetValue("Version", intVersion)

        End If
        reg_config.Close()
        DateTimePicker_jinyan.Value = DateAdd("m", 1, Date.Now)
        DateTimePicker_suoding.Value = DateAdd("m", 1, Date.Now)
        DateTimePicker_xingxiang.Value = DateAdd("m", 1, Date.Now)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\PvPGN GLQ")
            load_config()
        Catch ex As Exception

        End Try

    End Sub









    Private Sub Button_bak_pvpgn_sql_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_bak_pvpgn_sql.Click
        Dim bakdatestr As String
        bakdatestr = Format(Now, "yyyy-MM-dd_HH.mm")

        Try
            Shell("d:\pvpgn\mysql\bin\mysqldump.exe --host=" + TextBox_sql_serverip.Text + " --user=" + TextBox_sql_root.Text + " --password=" + TextBox_sql_password.Text + " --databases pvpgn --result-file=d:\pvpgn\databak\sqlbak" + bakdatestr + ".sql", AppWinStyle.Hide, True)
            MessageBox.Show("备份数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timer_dingshirenwu.Tick

    End Sub

    Private Sub TextBox_auto_lock_day_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox_auto_lock_day.KeyUp
        Dim aa
        aa = TextBox_auto_lock_day.Text
        If Not IsNumeric(TextBox_auto_lock_day.Text) Then
            MsgBox("只能输入数字")
            TextBox_auto_lock_day.Text = ""
            TextBox_auto_lock_day.Refresh()
        Else
            TextBox_auto_lock_day.Text = aa
        End If
    End Sub




    'Private Sub flag_no1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles flag_no1.SelectedValueChanged
    '    If flag_no1.Text = "0x200000 PGL玩家" Then
    '        flag1 = "2"
    '    Else
    '        flag1 = "0"
    '    End If
    '    flag_no.Text = flag1 + flag2 + flag3 + flag4 + flag5 + flag6 + flag7
    'End Sub




    'Private Sub flag_no2_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles flag_no2.SelectedValueChanged
    '    Select Case flag_no2.Text
    '        Case "0x0100000 GF官员"
    '            flag2 = "1"
    '        Case "0x0200000 GF玩家"
    '            flag2 = "2"
    '        Case Else
    '            flag2 = "0"
    '    End Select
    '    flag_no.Text = flag1 + flag2 + flag3 + flag4 + flag5 + flag6 + flag7
    'End Sub



    'Private Sub flag_no3_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles flag_no3.SelectedValueChanged
    '    Select Case flag_no3.Text
    '        Case "0x0010000 KBK新手"
    '            flag3 = "1"
    '        Case "0x0020000 White KBK (1 bar)"
    '            flag3 = "2"
    '        Case Else
    '            flag3 = 0
    '    End Select
    '    flag_no.Text = flag1 + flag2 + flag3 + flag4 + flag5 + flag6 + flag7
    'End Sub



    'Private Sub flag_no4_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles flag_no4.SelectedValueChanged
    '    Select Case flag_no4.Text
    '        Case "0x0001000 WCG官员"
    '            flag4 = "1"
    '        Case "0x0002000 KBK单人"
    '            flag4 = "2"
    '        Case Else
    '            flag4 = "0"
    '    End Select
    '    flag_no.Text = flag1 + flag2 + flag3 + flag4 + flag5 + flag6 + flag7
    'End Sub

    'Private Sub flag_no5_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles flag_no5.SelectedValueChanged
    '    Select Case flag_no5.Text
    '        Case "0x0000100 开启警报"
    '            flag5 = "1"
    '        Case "0x0000200 PGL玩家"
    '            flag5 = "2"
    '        Case "0x0000400 PGL官员"
    '            flag5 = "4"
    '        Case "0x0000800 KBK玩家"
    '            flag5 = "8"
    '        Case Else
    '            flag5 = "0"
    '    End Select
    '    flag_no.Text = flag1 + flag2 + flag3 + flag4 + flag5 + flag6 + flag7
    'End Sub



    'Private Sub flag_no6_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles flag_no6.SelectedValueChanged
    '    Select Case flag_no6.Text
    '        Case "0x0000010 不支持UDP"
    '            flag6 = "1"
    '        Case "0x0000020 光环（压制）"
    '            flag6 = "2"
    '        Case "0x0000040 特别来宾"
    '            flag6 = "4"
    '        Case "0x0000080 未知（测试）"
    '            flag6 = "8"
    '        Case Else
    '            flag6 = "0"
    '    End Select
    '    flag_no.Text = flag1 + flag2 + flag3 + flag4 + flag5 + flag6 + flag7
    'End Sub



    Private Sub flag_no7_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles flag_no7.SelectedValueChanged
        Select Case flag_no7.Text
            Case "暴雪代表(admin)"
                flag5 = "0"
                flag7 = "1"
            Case "频道管理员(锤子)"
                flag5 = "0"
                flag7 = "2"
            Case "公告员(铃铛)"
                flag5 = "0"
                flag7 = "4"
            Case "战网管理员(书生)"
                flag5 = "0"
                flag7 = "8"
            Case "官员(红袍)"
                flag5 = "4"
                flag7 = "0"
            Case Else
                flag5 = "0"
                flag7 = "0"
        End Select
    End Sub



    Private Sub Button_fix_pvpgn_server_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_fix_pvpgn_server.Click
        Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\pvpgn", "DependOnService", New String() {"MySQL56"}, Microsoft.Win32.RegistryValueKind.MultiString)
        MsgBox("修正成功")
    End Sub

    Private Sub Label44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label44.Click

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_add_flags_exp_date.Click
        'Dim pathbnet As New MySqlCommand("ALTER TABLE `pvpgn_bnet` ADD COLUMN `flags_exp_date` date NULL;", conn)
        'pathbnet.ExecuteNonQuery()
        'MsgBox("数据库已添加频道形象定时功能。")

        'Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        'Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        'If reg_config Is Nothing Then
        '    reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
        'End If
        'If reg_config IsNot Nothing Then
        '    reg_config.SetValue("添加形象定时功能", "1")
        'End If
        'reg_config.Close()
        'showbutton()
    End Sub

    Private Sub Button_add_unset_lock_exp_date_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_add_unset_lock_exp_date.Click
        'Dim pathbnet As New MySqlCommand("ALTER TABLE `pvpgn_bnet` ADD COLUMN `lockk_exp_date` date NULL;", conn)
        'pathbnet.ExecuteNonQuery()
        'MsgBox("数据库已添加锁定定时功能。")

        'Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        'Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        'If reg_config Is Nothing Then
        '    reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
        'End If
        'If reg_config IsNot Nothing Then
        '    reg_config.SetValue("添加锁定定时功能", "1")
        'End If
        'reg_config.Close()
        'showbutton()
    End Sub

    Private Sub Button_add_unset_mute_exp_date_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_add_unset_mute_exp_date.Click
        'Dim pathbnet As New MySqlCommand("ALTER TABLE `pvpgn_bnet` ADD COLUMN `mute_exp_date` date NULL;", conn)
        'pathbnet.ExecuteNonQuery()
        'MsgBox("数据库已添加禁言定时功能。")
        'Dim reg_path = "SOFTWARE\\PvPGN GLQ"
        'Dim reg_config = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(reg_path, True)
        'If reg_config Is Nothing Then
        '    reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
        'End If
        'If reg_config IsNot Nothing Then
        '    reg_config.SetValue("添加禁言定时功能", "1")
        'End If
        'reg_config.Close()
        'showbutton()
    End Sub

    Private Sub Button_con_to_sql_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_con_to_sql.EnabledChanged
        showbutton()
    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub
End Class
