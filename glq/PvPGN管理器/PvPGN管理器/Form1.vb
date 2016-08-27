Imports System
Imports System.Data
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports System.Management
Imports System.ServiceProcess
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form_main
    Inherits System.Windows.Forms.Form
    Dim a0 As DateTime = #6/13/2010 1:00:00 PM#
    Dim a1 As DateTime = #6/13/2010 1:00:20 PM#
    Dim outtime As TimeSpan = a1 - a0
    Dim conn As MySqlConnection
    Dim data As DataTable
    Dim da As MySqlDataAdapter
    Dim cb As MySqlCommandBuilder
    Dim d2cs_server_string As String
    Dim d2dbs_server_string As String
    Dim os_ver As String
    Dim flag1 As String = "0"
    Dim flag2 As String = "0"
    Dim flag3 As String = "0"
    Dim flag4 As String = "0"
    Dim flag5 As String = "0"
    Dim flag6 As String = "0"
    Dim flag7 As String = "0"
    Dim d2ver As String = "0"
    Dim server_status As String
    Dim server_name As String

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

                '已经连接到pvpgn
            ElseIf TextBox_database_name.Text = "pvpgn" Then
                Button_close_sql.Enabled = True
                'Button_del_pvpgn_sql.Enabled = True
                Button_mysql_data_backup.Enabled = True
                Button_mysql_data_restore.Enabled = True
                CheckBox_timer_backup.Enabled = True
                CheckBox_timer_autolock.Enabled = True
                If reg_config Is Nothing Then
                    reg_config = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(reg_path)
                End If
                If reg_config IsNot Nothing Then
                    If reg_config.GetValue("添加形象功能", "0") = "0" Then
                        Button_add_flags.Enabled = True
                    Else
                        Button_add_flags.Enabled = False
                    End If

                    If reg_config.GetValue("添加形象定时功能", "0") = "0" Then
                        Button_add_flags_exp_date.Enabled = True
                    Else
                        Button_add_flags_exp_date.Enabled = False
                    End If

                    If reg_config.GetValue("添加锁定定时功能", "0") = "0" Then
                        Button_add_unset_lock_exp_date.Enabled = True
                    Else
                        Button_add_unset_lock_exp_date.Enabled = False
                    End If

                    If reg_config.GetValue("添加禁言定时功能", "0") = "0" Then
                        Button_add_unset_mute_exp_date.Enabled = True
                    Else
                        Button_add_unset_mute_exp_date.Enabled = False
                    End If
                End If
            End If



            '状态为断开连接的话
        Else
            Button_close_sql.Enabled = False

            'Button_del_pvpgn_sql.Enabled = False
            Button_add_flags.Enabled = False
            Button_add_flags_exp_date.Enabled = False
            Button_add_unset_lock_exp_date.Enabled = False
            Button_add_unset_mute_exp_date.Enabled = False
            Button_mysql_data_restore.Enabled = False
            Button_mysql_data_backup.Enabled = False

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
            If RadioButton_win_ver_2012.Checked = True Then
                reg_config.SetValue("RadioButton_win_ver_2012.Checked", "1")
            Else
                reg_config.SetValue("RadioButton_win_ver_2012.Checked", "0")
            End If

            If RadioButton_d2_113C.Checked = True Then
                reg_config.SetValue("RadioButton_d2_113C.Checked", "1")
            Else
                reg_config.SetValue("RadioButton_d2_113C.Checked", "0")
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

        If DateString > "2018-3-30" Then
            Close()
        End If
        load_config()
        d2gsver()
        shuaxin()
        '自动连接数据库
        If CheckBox_save_password.Checked = True Then
            If Not conn Is Nothing Then conn.Close()
            Dim connStr As String
            connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=False",
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
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=False",
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
        'Dim selectpvpgn As New MySqlCommand("Select * FROM `pvpgn_bnet` LIMIT 0, 1000", conn)
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

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_pvpgn_install.Click
        'MessageBox.Show("安装开始后会出现CMD窗口，当显示""Installing Service""后关掉CMD窗口")
        Shell("cmd /c d:\pvpgn\PvPGNConsole.exe -s install", AppWinStyle.Hide, True)
        Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\pvpgn", "DependOnService", New String() {"MySQL"}, Microsoft.Win32.RegistryValueKind.MultiString)
        'MessageBox.Show(i)
        MessageBox.Show("PvPGN已安装")
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_pvpgn_uninstall.Click
        Dim sspvpgn As New ServiceController("pvpgn")
        If sspvpgn.Status.Equals(ServiceControllerStatus.Running) Then
            MessageBox.Show("请停止PvPGN后重试")
        Else
            Shell("PvPGNConsole.exe -s uninstall", vbHide)
            MessageBox.Show("卸载完成")
        End If
    End Sub

    'Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim sspvpgn As New ServiceController("pvpgn")
    '    sspvpgn.Refresh()
    '    MessageBox.Show("已重启")
    'End Sub

    'Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim ssd2cs As New ServiceController("d2cs")
    '    ssd2cs.Refresh()
    '    MessageBox.Show("已重启")
    'End Sub

    'Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim ssd2dbs As New ServiceController("d2dbs")
    '    ssd2dbs.Refresh()
    '    MessageBox.Show("已重启")
    'End Sub

    'Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim sspvpgn As New ServiceController("pvpgn")
    '    Dim ssd2cs As New ServiceController("d2cs")
    '    Dim ssd2dbs As New ServiceController("d2dbs")
    '    sspvpgn.Refresh()
    '    ssd2cs.Refresh()
    '    ssd2dbs.Refresh()
    '    MessageBox.Show("已全部重启")
    'End Sub


    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2files_copy.Click
        Button_d2files_copy.Text = "正在复制"
        Button_d2files_copy.Enabled = False
        ProgressBar1.Visible = True
        ProgressBar1.Value = 0
        Dim i
        Dim d2gsneedfiles(4) As String
        d2gsneedfiles(0) = "d2data.mpq"
        d2gsneedfiles(1) = "d2exp.mpq"
        d2gsneedfiles(2) = "d2speech.mpq"
        d2gsneedfiles(3) = "d2sfx.mpq"
        d2gsneedfiles(4) = "patch_d2.mpq"
        For i = 0 To 4
            If System.IO.File.Exists(TextBox_d2_path.Text + "\" + d2gsneedfiles(i)) Then
                ProgressBar1.Value = ProgressBar1.Value + 18
                System.IO.File.Copy(TextBox_d2_path.Text + "\" + d2gsneedfiles(i), "d:\pvpgn\d2gs\" + d2ver + "\" + d2gsneedfiles(i), True)
            Else
                MsgBox(d2gsneedfiles(i) + "没有找到")
            End If
        Next
        ProgressBar1.Value = 100
        ProgressBar1.Visible = False
        Button_d2files_copy.Text = "复制所需文件"
        MsgBox("复制完成")
        Button_d2files_copy.Enabled = True
    End Sub

    Private Sub d2gsdefconf()
        '兼容性
        If os_ver = "win2012" Then
            Dim jianrongxing As String = "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"
            Microsoft.Win32.Registry.SetValue(jianrongxing, "D:\pvpgn\d2gs\" & d2ver & "\D2GS.exe", "~ RUNASADMIN WIN7RTM")
            'Microsoft.Win32.Registry.SetValue(jianrongxing, "D:\pvpgn\d2gs\1.13c\D2GS.exe", "~ RUNASADMIN WIN7RTM")
        Else

        End If

        '32位兼容
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
        '64位兼容
        d2gsregname = "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\D2Server\D2GS"
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


    Private Sub Button35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_mysql_data_restore.Click
        If TextBox_sqlbak_name.Text = "" Then
            MsgBox("请先选择需要还原的数据文件。")
        Else
            Try
                Shell("cmd /c d:\pvpgn\mysql\bin\mysql.exe --host=" + TextBox_sql_serverip.Text + " --user=" + TextBox_sql_root.Text + " --password=" + TextBox_sql_password.Text + " pvpgn < " + TextBox_sqlbak_name.Text, AppWinStyle.Hide, True)
                MessageBox.Show("还原数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If


    End Sub

    Private Sub Button36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2cs_install.Click
        Shell("cmd /c d:\pvpgn\" & d2cs_server_string & "Console.exe -s install", vbHide, True)
        'MessageBox.Show(i)
        MsgBox(d2cs_server_string & "已安装")
    End Sub

    Private Sub Button37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2dbs_install.Click
        Shell("cmd /c d:\pvpgn\" & d2dbs_server_string + "Console.exe -s install", vbHide， True)
        'MessageBox.Show(i)
        MsgBox(d2dbs_server_string & "D2DBS已安装")
    End Sub

    Private Sub Button38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2cs_uninstall.Click
        Dim ssd2cs As New ServiceController(d2cs_server_string)
        If ssd2cs.Status.Equals(ServiceControllerStatus.Running) Then
            MessageBox.Show("请停止D2CS服务后重试")
        Else
            Shell("cmd /c " + d2cs_server_string + "Console.exe -s uninstall", AppWinStyle.Hide, True)
            'MessageBox.Show(i)
            MessageBox.Show("卸载完成")
        End If
    End Sub

    Private Sub Button39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2dbs_uninstall.Click
        Dim ssd2dbs As New ServiceController(d2dbs_server_string)

        If ssd2dbs.Status.Equals(ServiceControllerStatus.Running) Then
            MessageBox.Show("请停止D2DBS服务后重试")
        Else
            Shell("cmd /c" + d2dbs_server_string + "Console.exe -s uninstall", vbHide)
            'MessageBox.Show(i)
            MessageBox.Show("卸载完成")
        End If
    End Sub


    'Public Sub stop_pvpgn_server()
    '    Dim sspvpgn As New ServiceController("pvpgn")
    '    Try
    '        If sspvpgn.Status <> ServiceControllerStatus.Stopped Then
    '            sspvpgn.Stop()
    '            sspvpgn.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
    '        End If
    '    Catch ex As Exception
    '        MsgBox("PvPGN未能停止，请重试，多次重试仍然不行请重启计算机")
    '    End Try
    'End Sub
    'Public Sub stop_mysql_server()
    '    Dim ssmysql As New ServiceController("mysql")
    '    Try
    '        If ssmysql.Status <> ServiceControllerStatus.Stopped Then
    '            ssmysql.Stop()
    '            ssmysql.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
    '        End If
    '    Catch ex As Exception
    '        MsgBox("MySQL未能停止，请重试，多次重试仍然不行请重启计算机")
    '    End Try
    'End Sub

    'Public Sub run_pvpgn_server()
    '    Dim sspvpgn As New ServiceController("pvpgn")
    '    Try
    '        sspvpgn.Start()
    '        sspvpgn.WaitForStatus(ServiceControllerStatus.Running, outtime)
    '    Catch When sspvpgn.Status = ServiceControllerStatus.Running
    '        MessageBox.Show("PvPGN正在运行，不能重复启动")
    '        Exit Sub
    '    Catch ex As Exception
    '        MessageBox.Show("不能启动PvPGN,请检查bnet.conf各项配置")
    '        Exit Sub
    '    End Try
    'End Sub

    'Public Sub run_mysql_server()
    '    Dim rsmysql As New ServiceController("MySQL")
    '    Try
    '        rsmysql.Start()
    '        rsmysql.WaitForStatus(ServiceControllerStatus.Running, outtime)
    '    Catch When rsmysql.Status = ServiceControllerStatus.Running
    '        MessageBox.Show("MySQL正在运行，不要重复启动。")
    '        Exit Sub
    '    Catch ex As Exception
    '        MessageBox.Show("不能启动MySQL服务，请重试。")
    '        Exit Sub
    '    End Try
    'End Sub

    'Public Sub stop_d2cs_server()
    '    Dim ssd2cs As New ServiceController(d2cs_server_string)
    '    Try
    '        If ssd2cs.Status <> ServiceControllerStatus.Stopped Then

    '            ssd2cs.Stop()
    '            ssd2cs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
    '        End If
    '    Catch ex As Exception
    '        MsgBox("D2CS未能停止，请重试，多次重试仍然不行请重启计算机")
    '    End Try
    'End Sub

    'Public Sub run_d2cs_server()
    '    Dim ssd2cs As New ServiceController(d2cs_server_string)
    '    Try
    '        ssd2cs.Start()
    '        ssd2cs.WaitForStatus(ServiceControllerStatus.Running, outtime)
    '    Catch When ssd2cs.Status = ServiceControllerStatus.Running
    '        MessageBox.Show("D2CS正在运行，不能重复启动")
    '        Exit Sub
    '    Catch ex As Exception
    '        MessageBox.Show("不能启动D2CS,请检查d2cs.conf各项配置")
    '        Exit Sub
    '    End Try
    'End Sub

    'Public Sub stop_d2dbs_server()
    '    Dim ssd2dbs As New ServiceController(d2dbs_server_string)
    '    Try
    '        If ssd2dbs.Status <> ServiceControllerStatus.Stopped Then
    '            ssd2dbs.Stop()
    '            ssd2dbs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
    '        End If
    '    Catch ex As Exception
    '        MsgBox("D2DBS未能停止，请重试，多次重试仍然不行请重启计算机")
    '    End Try
    'End Sub

    'Public Sub run_d2dbs_server()
    '    Dim ssd2dbs As New ServiceController(d2dbs_server_string)
    '    Try
    '        ssd2dbs.Start()
    '        ssd2dbs.WaitForStatus(ServiceControllerStatus.Running, outtime)
    '    Catch When ssd2dbs.Status = ServiceControllerStatus.Running
    '        MessageBox.Show("D2DBS正在运行，不能重复启动")
    '        Exit Sub
    '    Catch ex As Exception
    '        MessageBox.Show("不能启动D2DBS,请检查d2dbs.conf各项配置")
    '        Exit Sub
    '    End Try
    'End Sub

    'Public Sub stop_d2gs_server()
    '    Dim ssd2gs As New ServiceController("d2gs")
    '    Try
    '        If ssd2gs.Status <> ServiceControllerStatus.Stopped Then
    '            ssd2gs.Stop()
    '            ssd2gs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
    '        End If
    '    Catch ex As Exception
    '        MsgBox("D2GS未能停止，请重试，多次重试仍然不行请重启计算机")
    '    End Try
    'End Sub

    ''Public Sub run_d2gs_server()
    ''    Dim ssd2gs As New ServiceController("d2gs")
    ''    Try
    ''        ssd2gs.Start()
    ''        ssd2gs.WaitForStatus(ServiceControllerStatus.Running, outtime)
    ''    Catch When ssd2gs.Status = ServiceControllerStatus.Running
    ''        MessageBox.Show("D2GS正在运行，不能重复启动")
    ''        Exit Sub
    ''    Catch ex As Exception
    ''        MessageBox.Show("不能启动D2GS,请检查D2GS配置")
    ''        Exit Sub
    ''    End Try
    ''End Sub

    Private Sub Button_restart_pvpgn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_pvpgn_restart.Click
        Button_pvpgn_restart.Text = "正在重启"
        Button_pvpgn_restart.Enabled = False

        server_name = "pvpgn"
        server_stop(server_name)
        server_run(server_name)

        'Label_server_pvpgn_status.Text = server_status
        shuaxin()

        Button_pvpgn_restart.Enabled = Enabled
        Button_pvpgn_restart.Text = "重启"
    End Sub

    Private Sub Button_restart_d2cs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2cs_restart.Click
        'stop_d2cs_server()
        'run_d2cs_server()
        'MessageBox.Show("重启指令执行完毕。")
        'shuaxin()
        Button_d2cs_restart.Text = "正在重启"
        Button_d2cs_restart.Enabled = False

        server_name = d2cs_server_string
        server_stop(server_name)
        server_run(server_name)

        'Label_server_d2cs_status.Text = server_status
        shuaxin()

        Button_d2cs_restart.Enabled = Enabled
        Button_d2cs_restart.Text = "重启"
    End Sub


    Private Sub Button_restart_d2dbs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2dbs_restart.Click
        'stop_d2dbs_server()
        'run_d2dbs_server()
        'MessageBox.Show("重启指令执行完毕。")
        'shuaxin()
        Button_d2dbs_restart.Text = "正在重启"
        Button_d2dbs_restart.Enabled = False

        server_name = d2dbs_server_string
        server_stop(server_name)
        server_run(server_name)

        'Label_server_d2dbs_status.Text = server_status
        shuaxin()

        Button_d2dbs_restart.Enabled = Enabled
        Button_d2dbs_restart.Text = "重启"
    End Sub

    Private Sub Button_restart_d2gs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2gs_restart.Click
        'stop_d2gs_server()
        'run_d2gs_server()
        'MessageBox.Show("重启指令执行完毕。")
        'shuaxin()
        Button_d2gs_restart.Text = "正在重启"
        Button_d2gs_restart.Enabled = False


        server_name = "d2gs"
        server_stop(server_name)
        server_run(server_name)

        'Label_server_d2gs_status.Text = server_status
        shuaxin()

        Button_d2gs_restart.Enabled = Enabled
        Button_d2gs_restart.Text = "重启"
    End Sub

    Private Sub Button_stop_pvpgn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_pvpgn_stop.Click
        'stop_pvpgn_server()
        'MessageBox.Show("停止指令执行完毕。")
        'shuaxin()
        Button_pvpgn_stop.Text = "正在停止"
        Button_pvpgn_stop.Enabled = False

        server_name = "pvpgn"
        server_stop(server_name)

        'Label_server_pvpgn_status.Text = server_status
        shuaxin()

        Button_pvpgn_restart.Enabled = Enabled
        Button_pvpgn_restart.Text = "停止"
    End Sub

    Private Sub Button_stop_d2cs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2cs_stop.Click
        'stop_d2cs_server()
        'MessageBox.Show("停止指令执行完毕。")
        'shuaxin()
        Button_d2cs_stop.Text = "正在停止"
        Button_d2cs_stop.Enabled = False

        server_name = d2cs_server_string
        server_stop(server_name)

        shuaxin()

        Button_d2cs_restart.Enabled = Enabled
        Button_d2cs_restart.Text = "停止"
    End Sub

    Private Sub Button_stop_d2dbs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2dbs_stop.Click
        'stop_d2dbs_server()
        'MessageBox.Show("停止指令执行完毕。")
        'shuaxin()
        Button_d2dbs_stop.Text = "正在停止"
        Button_d2dbs_stop.Enabled = False

        server_name = d2dbs_server_string
        server_stop(server_name)

        shuaxin()

        Button_d2dbs_restart.Enabled = Enabled
        Button_d2dbs_restart.Text = "停止"
    End Sub

    Private Sub Button_stop_d2gs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2gs_stop.Click
        'stop_d2gs_server()
        'MessageBox.Show("停止指令执行完毕。")
        'shuaxin()
        Button_d2gs_stop.Text = "正在停止"
        Button_d2gs_stop.Enabled = False

        server_name = "d2gs"
        server_stop(server_name)

        shuaxin()

        Button_d2gs_restart.Enabled = Enabled
        Button_d2gs_restart.Text = "停止"

    End Sub

    Private Sub Button_stop_select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_select_server_stop.Click
        Button_select_server_stop.Text = "正在停止"
        Button_select_server_stop.Enabled = False
        If CheckBox_pvpgn.Checked Then
            'stop_pvpgn_server()

            server_name = "pvpgn"
            server_stop(server_name)
        End If

        If CheckBox_d2cs.Checked Then
            'stop_d2cs_server()
            server_name = d2cs_server_string
            server_stop(server_name)
        End If

        If CheckBox_d2dbs.Checked Then
            'stop_d2dbs_server()
            server_name = d2dbs_server_string
            server_stop(server_name)
        End If

        If CheckBox_d2gs.Checked Then
            'stop_d2gs_server()
            server_name = "d2gs"
            server_stop(server_name)
        End If

        'MsgBox("停止指令执行完毕。")
        Button_select_server_stop.Enabled = Enabled
        Button_select_server_stop.Text = "停止指定服务"
        shuaxin()
    End Sub

    Private Sub Button_restart_select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_select_server_restart.Click
        Button_select_server_restart.Text = "正在重启"
        Button_select_server_restart.Enabled = False
        If CheckBox_pvpgn.Checked = True Then
            'stop_pvpgn_server()
            'run_pvpgn_server()
            server_name = "pvpgn"
            server_stop(server_name)
            server_run(server_name)

        End If

        If CheckBox_d2cs.Checked = True Then
            'stop_d2cs_server()
            'run_d2cs_server()
            server_name = d2cs_server_string
            server_stop(server_name)
            server_run(server_name)
        End If

        If CheckBox_d2dbs.Checked = True Then
            'stop_d2dbs_server()
            'run_d2dbs_server()
            server_name = d2dbs_server_string
            server_stop(server_name)
            server_run(server_name)
        End If

        If CheckBox_d2gs.Checked = True Then
            'stop_d2gs_server()
            'run_d2gs_server()
            server_name = "d2gs"
            server_stop(server_name)
            server_run(server_name)
        End If
        'MessageBox.Show("重启指令执行完毕。")

        Button_select_server_restart.Enabled = Enabled
        Button_select_server_restart.Text = "重启指定服务"
        shuaxin()
    End Sub

    Private Sub Button_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_refresh.Click
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
                    Label_server_pvpgn_status.Text = "正在运行"
                Case ServiceControllerStatus.Stopped
                    Label_server_pvpgn_status.Text = "已停止"
            End Select
        Catch ex As Exception

        End Try

        Try
            Select Case ssd2cs.Status
                Case ServiceControllerStatus.Running
                    Label_server_d2cs_status.Text = "正在运行"
                Case ServiceControllerStatus.Stopped
                    Label_server_d2cs_status.Text = "已停止"
            End Select
        Catch ex As Exception

        End Try

        Try
            Select Case ssd2dbs.Status
                Case ServiceControllerStatus.Running
                    Label_server_d2dbs_status.Text = "正在运行"
                Case ServiceControllerStatus.Stopped
                    Label_server_d2dbs_status.Text = "已停止"
            End Select
        Catch ex As Exception

        End Try

        Try
            Select Case ssd2gs.Status
                Case ServiceControllerStatus.Running
                    Label_server_d2gs_status.Text = "正在运行"
                Case ServiceControllerStatus.Stopped
                    Label_server_d2gs_status.Text = "已停止"
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

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_d2dir_brower.Click
        FolderBrowserDialog_diabloII_dir.ShowDialog()
        TextBox_d2_path.Text = FolderBrowserDialog_diabloII_dir.SelectedPath

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_close_sql.Click
        conn.Close()
        Button_con_to_sql.Enabled = True
        showbutton()
    End Sub


    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        OpenFileDialog_mysqlbakfiles_name.ShowDialog()
        TextBox_sqlbak_name.Text = OpenFileDialog_mysqlbakfiles_name.FileName
    End Sub

    Private Sub d2gsver()
        If RadioButton_d2_109.Checked = True Then
            d2cs_server_string = "d2cs109"
            d2dbs_server_string = "d2dbs109"
            d2ver = "1.09d"

        Else
            d2cs_server_string = "d2cs"
            d2dbs_server_string = "d2dbs"
            d2ver = "1.13c"
        End If
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

            If reg_config.GetValue("RadioButton_win_ver_2012.Checked", "1") = "1" Then
                RadioButton_win_ver_2012.Checked = True
                os_ver = "win2012"
            Else
                RadioButton_win_ver_2003.Checked = True
                os_ver = "win2003"
            End If

            If reg_config.GetValue("RadioButton_d2_113C.Checked", "1") = "1" Then
                RadioButton_d2_113C.Checked = True
                d2cs_server_string = "d2cs"
                d2dbs_server_string = "d2dbs"
                d2ver = "1.13c"
            Else
                RadioButton_d2_109.Checked = True
                d2cs_server_string = "d2cs109"
                d2dbs_server_string = "d2dbs109"
                d2ver = "1.09d"
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

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_glq_config_restart.Click
        Try
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\PvPGN GLQ")
            load_config()
        Catch ex As Exception

        End Try

    End Sub









    Private Sub Button_bak_pvpgn_sql_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_mysql_data_backup.Click
        Dim bakdatestr As String
        bakdatestr = Format(Now, "yyyy-MM-dd_HH.mm")

        Try
            Shell("cmd /c d:\pvpgn\mysql\bin\mysqldump.exe --host=" + TextBox_sql_serverip.Text + " --user=" + TextBox_sql_root.Text + " --password=" + TextBox_sql_password.Text + " --databases pvpgn --result-file=d:\pvpgn\databak\sqlbak" + bakdatestr + ".sql", AppWinStyle.Hide, True)
            MessageBox.Show("备份数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timer_dingshirenwu.Tick
        Dim time_hm As String
        time_hm = Now.Hour.ToString + Now.Minute.ToString

        '备份数据库
        If CheckBox_timer_backup.Checked = True And CheckBox_timer_backup.Enabled = True And ComboBox_backup_h.Text + ComboBox_backup_m.Text = time_hm Then

            Dim bakdatestr As String
            bakdatestr = Format(Now, "yyyy-MM-dd_HH.mm")

            Try
                Shell("cmd /c d:\pvpgn\mysql\bin\mysqldump.exe --host=" + TextBox_sql_serverip.Text + " --user=" + TextBox_sql_root.Text + " --password=" + TextBox_sql_password.Text + " --databases pvpgn --result-file=d:\pvpgn\databak\sqlbak" + bakdatestr + ".sql", AppWinStyle.Hide, True)
                'MessageBox.Show("备份数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
            End Try

        End If
        '备份数据库结束

        '锁定用户
        If CheckBox_timer_autolock.Checked = True And CheckBox_timer_autolock.Enabled = True And ComboBox_auto_lock_houre.Text + ComboBox_auto_lock_m.Text = time_hm Then
            Dim d1970 As New System.DateTime(1970, 1, 1, 0, 0, 0, 0)
            Dim iSeconds As Long
            iSeconds = (Now.Ticks - d1970.Ticks) / 10000000
            Dim lock_day_to_m As Long
            lock_day_to_m = Val(TextBox_auto_lock_day.Text) * 24 * 60 * 60
            Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
            Dim set_lockk_str As String
            set_lockk_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_lockk`='1' WHERE ('{0}' - `acct_lastlogin_time` > '{1}') LIMIT 1000", iSeconds, lock_day_to_m)
            Dim set_lockk As New MySqlCommand(set_lockk_str, conn)
            selectpvpgn.ExecuteNonQuery()
            set_lockk.ExecuteNonQuery()
            Label_timer_zhuangtai.Text = "于" + time_hm + "执行锁定任务。"
        End If

        '锁定用户停止

        '停止服务
        If CheckBox_timer_stop_pvpgn.Checked = True And ComboBox_stop_pvpgn_houre.Text + ComboBox_stop_pvpgn_m.Text = time_hm Then
            If CheckBox_pvpgn.Checked = True Then
                Dim sspvpgn As New ServiceController("pvpgn")
                Try
                    If sspvpgn.Status <> ServiceControllerStatus.Stopped Then
                        sspvpgn.Stop()
                        sspvpgn.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
                        Label_timer_zhuangtai.Text = "于" + time_hm + "停止成功"
                    End If
                Catch ex As Exception
                    Label_timer_zhuangtai.Text = "于" + time_hm + "停止失败"
                End Try
            End If

            If CheckBox_d2cs.Checked = True Then
                Dim ssd2cs As New ServiceController(d2cs_server_string)
                Try
                    If ssd2cs.Status <> ServiceControllerStatus.Stopped Then
                        ssd2cs.Stop()
                        ssd2cs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
                        Label_timer_zhuangtai.Text = "于" + time_hm + "停止成功"
                    End If
                Catch ex As Exception
                    Label_timer_zhuangtai.Text = "于" + time_hm + "停止失败"
                End Try
            End If

            If CheckBox_d2dbs.Checked = True Then
                Dim ssd2dbs As New ServiceController(d2dbs_server_string)
                Try
                    If ssd2dbs.Status <> ServiceControllerStatus.Stopped Then
                        ssd2dbs.Stop()
                        ssd2dbs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
                        Label_timer_zhuangtai.Text = "于" + time_hm + "停止成功"
                    End If
                Catch ex As Exception
                    Label_timer_zhuangtai.Text = "于" + time_hm + "停止失败"
                End Try
            End If

            If CheckBox_d2gs.Checked = True Then
                Dim ssd2gs As New ServiceController("d2gs")
                Try
                    If ssd2gs.Status <> ServiceControllerStatus.Stopped Then
                        ssd2gs.Stop()
                        ssd2gs.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
                        Label_timer_zhuangtai.Text = "于" + time_hm + "停止成功"
                    End If
                Catch ex As Exception
                    Label_timer_zhuangtai.Text = "于" + time_hm + "停止失败"
                End Try
            End If
            shuaxin()

            If CheckBox_re_jisuanji.Checked = True Then
                Shell("cmd /c shutdown.exe /r /t 30 /c ""PvPGN管理器定时重启"" /f", AppWinStyle.NormalFocus)
            End If
        End If
        '停止服务结束

        '启动服务
        If CheckBox_timer_re_pvpgn.Checked = True And ComboBox_re_pvpgn_houre.Text + ComboBox_re_pvpgn_m.Text = time_hm Then
            If CheckBox_pvpgn.Checked = True Then
                Dim sspvpgn As New ServiceController("pvpgn")
                Try
                    sspvpgn.Start()
                    sspvpgn.WaitForStatus(ServiceControllerStatus.Running, outtime)
                    Label_timer_zhuangtai.Text = "于" + time_hm + "启动成功"
                Catch When sspvpgn.Status = ServiceControllerStatus.Running
                    Exit Sub
                Catch ex As Exception
                    Label_timer_zhuangtai.Text = "于" + time_hm + "停止失败"
                    Exit Sub
                End Try
            End If

            If CheckBox_d2cs.Checked = True Then
                Dim ssd2cs As New ServiceController(d2cs_server_string)
                Try
                    ssd2cs.Start()
                    ssd2cs.WaitForStatus(ServiceControllerStatus.Running, outtime)
                    Label_timer_zhuangtai.Text = "于" + time_hm + "启动成功"
                Catch When ssd2cs.Status = ServiceControllerStatus.Running
                    Exit Sub
                Catch ex As Exception
                    Label_timer_zhuangtai.Text = "于" + time_hm + "启动失败"
                    Exit Sub
                End Try
            End If

            If CheckBox_d2dbs.Checked = True Then
                Dim ssd2dbs As New ServiceController(d2dbs_server_string)
                Try
                    ssd2dbs.Start()
                    ssd2dbs.WaitForStatus(ServiceControllerStatus.Running, outtime)
                    Label_timer_zhuangtai.Text = "于" + time_hm + "启动成功"
                Catch When ssd2dbs.Status = ServiceControllerStatus.Running
                    Exit Sub
                Catch ex As Exception
                    Label_timer_zhuangtai.Text = "于" + time_hm + "启动失败"
                    Exit Sub
                End Try
            End If

            If CheckBox_d2gs.Checked = True Then
                Dim ssd2gs As New ServiceController("d2gs")
                Try
                    ssd2gs.Start()
                    ssd2gs.WaitForStatus(ServiceControllerStatus.Running, outtime)
                    Label_timer_zhuangtai.Text = "于" + time_hm + "启动成功"
                Catch When ssd2gs.Status = ServiceControllerStatus.Running
                    Exit Sub
                Catch ex As Exception
                    Label_timer_zhuangtai.Text = "于" + time_hm + "启动失败"
                    Exit Sub
                End Try
            End If
            shuaxin()
        End If
        '启动服务结束


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


    Private Sub Timer_exp_date_Tick(sender As Object, e As EventArgs) Handles Timer_exp_date.Tick
        '自动断开，再重连数据库，避免mysql判断超时断开连接。
        conn.Close()
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
            'Select Case ex.Number
            ' Case 0
            ' MessageBox.Show("账号密码不对")
            ' Case 1042
            '  MessageBox.Show("找不到服务器")

            ' End Select
            'MessageBox.Show(ex.Number)
            'MessageBox.Show(ex.Message)
        End Try
        '自动断开再重连结束

        Dim selectpvpgn As New MySqlCommand("SELECT * FROM `pvpgn_bnet` LIMIT 0, 3000", conn)
        '解除禁言
        Dim set_unmute_str As String
        set_unmute_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_mute`='0' WHERE (`mute_exp_date` <= '{0}') LIMIT 3000", Date.Now)
        Dim set_unmute As New MySqlCommand(set_unmute_str, conn)
        '解除锁定
        Dim set_unlock_str As String
        set_unlock_str = String.Format("UPDATE `pvpgn_bnet` SET `auth_lockk`='0' WHERE (`lockk_exp_date` <= '{0}') LIMIT 3000", Date.Now)
        Dim set_unlock As New MySqlCommand(set_unlock_str, conn)
        '去除形象
        Dim set_del_flags_str As String
        set_del_flags_str = String.Format("UPDATE `pvpgn_bnet` SET `flags_initial`='0' WHERE (`flags_exp_date` <= '{0}') LIMIT 3000", Date.Now)
        Dim set_del_flags As New MySqlCommand(set_del_flags_str, conn)
        '执行
        selectpvpgn.ExecuteNonQuery()
        set_unmute.ExecuteNonQuery()
        set_unlock.ExecuteNonQuery()
        set_del_flags.ExecuteNonQuery()
    End Sub

    Private Sub Button_mysql_config_Click(sender As Object, e As EventArgs) Handles Button_mysql_password_modify.Click
        If Button_con_to_sql.Enabled = True Then
            MsgBox("请先连接数据库。")
        Else
            If TextBox_mysql_password1.Text = "" Or TextBox_mysql_password2.Text = "" Or TextBox_mysql_password1.Text <> TextBox_mysql_password2.Text Then
                MsgBox("两次输入的密码不相同，或为空")
            Else
                Shell("cmd /c d:\pvpgn\mysql\bin\mysqladmin.exe -uroot -p" & TextBox_sql_password.Text & " password " & TextBox_mysql_password1.Text)
                TextBox_mysql_password1.Text = ""
                TextBox_mysql_password2.Text = ""
                '相当于断开按钮
                conn.Close()
                Button_con_to_sql.Enabled = True
                showbutton()
                '
                MsgBox("修改完毕，请尝试用新密码连接测试是否成功。")
            End If
        End If


    End Sub

    Private Sub TextBox_mysql_password1_TextChanged(sender As Object, e As EventArgs) Handles TextBox_mysql_password1.TextChanged

    End Sub

    Private Sub Button_install_mysql_Click(sender As Object, e As EventArgs) Handles Button_install_mysql.Click
        '相当于断开按钮
        Try
            conn.Close()
            Button_con_to_sql.Enabled = True
            showbutton()
        Catch ex As Exception

        End Try

        '

        Shell("cmd /c D:\pvpgn\mysql\instsql.bat", AppWinStyle.NormalFocus, True)
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

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button_mysql_uninstall.Click
        '相当于断开按钮
        Try
            conn.Close()
            Button_con_to_sql.Enabled = True
            showbutton()
        Catch ex As Exception

        End Try

        Shell("cmd /c D:\pvpgn\mysql\remove.bat", AppWinStyle.NormalFocus, True)
        MsgBox("数据库卸载完毕。")
    End Sub

    Private Sub RadioButton_d2_109_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_d2_109.CheckedChanged
        d2gsver()
    End Sub

    Private Sub RadioButton_d2_113C_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_d2_113C.CheckedChanged
        d2gsver()
    End Sub

    Private Sub Button_d2gs_install_Click(sender As Object, e As EventArgs) Handles Button_d2gs_install.Click
        d2gsdefconf()
        Shell("cmd /c d:\pvpgn\d2gs\" + d2ver + "\d2gssvc.exe -i", AppWinStyle.Hide, True)
        MsgBox("安装完成")
    End Sub

    Private Sub Button_d2gs_uninstall_Click(sender As Object, e As EventArgs) Handles Button_d2gs_uninstall.Click
        Dim ssd2gs As New ServiceController("d2gs")
        Try
            If ssd2gs.Status = ServiceControllerStatus.Running Then

                MessageBox.Show("请停止D2GS服务后重试")

            Else

                Try
                    My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\D2Server\D2GS")
                Catch ex As Exception
                    'MsgBox("清除注册表失败")
                End Try
                Try
                    My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\Wow6432Node\D2Server\D2GS")
                Catch ex As Exception
                End Try
                Try
                    My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\D2Server")
                Catch ex As Exception
                End Try
                Try
                    My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\Wow6432Node\D2Server")
                Catch ex As Exception
                End Try
                Try
                    My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", True).DeleteValue("D:\pvpgn\d2gs\" & d2ver & "\D2GS.exe")
                Catch ex As Exception
                End Try

                Try
                    Shell("cmd /c d:\pvpgn\d2gs\" + d2ver + "\d2gssvc.exe -u", AppWinStyle.Hide, True)
                    MsgBox("卸载完成")
                Catch ex As Exception
                    MsgBox("卸载服务失败")
                End Try

            End If

        Catch ex As Exception
            MsgBox("D2GS没有安装")
        End Try

    End Sub

    Private Sub RadioButton_win_ver_2003_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_win_ver_2003.CheckedChanged
        If RadioButton_win_ver_2012.Checked = True Then
            os_ver = "win2012"
        Else
            os_ver = "win2003"
        End If
    End Sub

    Private Sub RadioButton_win_ver_2012_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_win_ver_2012.CheckedChanged
        If RadioButton_win_ver_2012.Checked = True Then
            os_ver = "win2012"
        Else
            os_ver = "win2003"
        End If
    End Sub

    Private Sub Button_d2gs_config_Click(sender As Object, e As EventArgs) Handles Button_d2gs_config.Click
        'Dim maxgames As Integer
        'maxgames = CInt(TextBox10.Text)
        '用bnpass.exe生成temp.txt文件，再把文件读取后截取hash，赋值给gs_telnet_password_hash变量
        Dim gs_telnet_password_hash As String
        Shell("cmd /c d:\pvpgn\bnpass.exe " & TextBox_d2gsconfig_telnet_password.Text & " >temp.txt", AppWinStyle.Hide, True)
        gs_telnet_password_hash = Mid(My.Computer.FileSystem.ReadAllText("temp.txt"), 26, 40)
        My.Computer.FileSystem.DeleteFile("temp.txt")

        '32位兼容
        Dim d2gsregname As String = "HKEY_LOCAL_MACHINE\SOFTWARE\D2Server\D2GS"
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2CSIP", TextBox_d2gsconfig_d2csip.Text)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2DBSIP", TextBox_d2gsconfig_d2dbsip.Text)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxGames", TextBox_d2gsconfig_maxgame.Text, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxGameLife", TextBox_d2gsconfig_MaxGameLife.Text)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AdminPassword", gs_telnet_password_hash)
        '64位兼容
        d2gsregname = "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\D2Server\D2GS"
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2CSIP", TextBox_d2gsconfig_d2csip.Text)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "D2DBSIP", TextBox_d2gsconfig_d2dbsip.Text)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxGames", TextBox_d2gsconfig_maxgame.Text, Microsoft.Win32.RegistryValueKind.DWord)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "MaxGameLife", TextBox_d2gsconfig_MaxGameLife.Text)
        Microsoft.Win32.Registry.SetValue(d2gsregname, "AdminPassword", gs_telnet_password_hash)

        MsgBox("请重启D2GS，使新设置生效")
    End Sub

    Private Function server_run(server_name)

        Dim rs As New ServiceController(server_name)

        Try
            If rs.Status = ServiceControllerStatus.Running Then
                server_status = "服务已在运行"
            Else
                rs.Start()
                rs.WaitForStatus(ServiceControllerStatus.Running, outtime)
                server_status = "服务启动成功"
            End If
        Catch ex As Exception
            server_status = ex.Message
        End Try

        Return server_status

    End Function

    Private Function server_stop(server_name)

        Dim ss As New ServiceController(server_name)

        Try
            If ss.Status = ServiceControllerStatus.Stopped Then
                server_status = "服务没有运行"
            Else
                ss.Stop()
                ss.WaitForStatus(ServiceControllerStatus.Stopped, outtime)
                server_status = "服务停止成功"
            End If
        Catch ex As Exception
            server_status = ex.Message
        End Try
        Return server_status
    End Function




    Private Sub Button_test_Click(sender As Object, e As EventArgs) Handles Button_test.Click
        server_name = "d2gs"
        server_stop(server_name)
        MsgBox(server_status)
        'Message.Create(server_status, server_status, "wparm", "lparam")


    End Sub

    Private Sub GroupBox_win_ver_Enter(sender As Object, e As EventArgs) Handles GroupBox_win_ver.Enter

    End Sub

    Private Sub Button_pvpgn_config_modify_Click(sender As Object, e As EventArgs) Handles Button_pvpgn_config_modify.Click
        Shell("explorer.exe d:\pvpgn\conf\", AppWinStyle.MaximizedFocus)
    End Sub
End Class
