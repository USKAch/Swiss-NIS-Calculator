VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "ComDlg32.OCX"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.2#0"; "mscomctl.OCX"
Begin VB.MDIForm frmMain1 
   BackColor       =   &H8000000C&
   Caption         =   "Feldstärke"
   ClientHeight    =   5700
   ClientLeft      =   225
   ClientTop       =   870
   ClientWidth     =   8595
   Icon            =   "MDIForm1.frx":0000
   LockControls    =   -1  'True
   StartUpPosition =   3  'Windows-Standard
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   3120
      Top             =   720
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin MSComctlLib.StatusBar StatusBar1 
      Align           =   2  'Unten ausrichten
      Height          =   255
      Left            =   0
      TabIndex        =   0
      Top             =   5445
      Width           =   8595
      _ExtentX        =   15161
      _ExtentY        =   450
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   3
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            AutoSize        =   1
            Object.Width           =   11033
         EndProperty
         BeginProperty Panel2 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Style           =   6
            Object.Width           =   1764
            MinWidth        =   1764
            TextSave        =   "19.07.2023"
         EndProperty
         BeginProperty Panel3 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Style           =   5
            Object.Width           =   1764
            MinWidth        =   1764
            TextSave        =   "15:00"
         EndProperty
      EndProperty
   End
   Begin VB.Menu mnuFile 
      Caption         =   "1101"
      Begin VB.Menu mnuFilebar2 
         Caption         =   "-"
      End
      Begin VB.Menu PrintFormLoad 
         Caption         =   "1107"
      End
      Begin VB.Menu mnuFileprint 
         Caption         =   "1106"
      End
      Begin VB.Menu mnu_Antdaten_Print 
         Caption         =   "1121"
      End
      Begin VB.Menu mnu_Filebar3 
         Caption         =   "-"
      End
      Begin VB.Menu Druck_laden 
         Caption         =   "1103"
      End
      Begin VB.Menu Druckspeichern 
         Caption         =   "1104"
      End
      Begin VB.Menu löschen 
         Caption         =   "1105"
         Enabled         =   0   'False
         Visible         =   0   'False
      End
      Begin VB.Menu mnu_Filebar2 
         Caption         =   "-"
      End
      Begin VB.Menu Ant_Aufbereiten 
         Caption         =   "1108"
      End
      Begin VB.Menu mnuFilebar1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuFileExit 
         Caption         =   "1109"
      End
   End
   Begin VB.Menu mnu_Internet 
      Caption         =   "Internet"
      Begin VB.Menu mnu_AntKommission 
         Caption         =   "1119"
      End
      Begin VB.Menu mnu_Atennenbib 
         Caption         =   "1118"
      End
      Begin VB.Menu mnu_NISVerordnung 
         Caption         =   "1135"
      End
      Begin VB.Menu mnu_NISErlauterung1 
         Caption         =   "1140"
      End
   End
   Begin VB.Menu mnu_Text_pdf 
      Caption         =   "Files"
      Begin VB.Menu mnu_Antenne 
         Caption         =   "1131"
      End
      Begin VB.Menu mnu_Wegleitung 
         Caption         =   "1132"
      End
      Begin VB.Menu mnu_Formelblatt 
         Caption         =   "1133"
      End
      Begin VB.Menu mnu_Glossar 
         Caption         =   "1134"
         Visible         =   0   'False
      End
      Begin VB.Menu mnu_standortblatt 
         Caption         =   "1135"
      End
      Begin VB.Menu mnu_Beilage1 
         Caption         =   "1136"
      End
      Begin VB.Menu mnu_Beilage5 
         Caption         =   "1137"
      End
      Begin VB.Menu mnu_Beilage7 
         Caption         =   "1138"
      End
      Begin VB.Menu mnu_NISVerordnung1 
         Caption         =   "1139"
      End
      Begin VB.Menu mnu_NISErlauterung 
         Caption         =   "1140"
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "1110"
      Begin VB.Menu mnu_Progr_Beschreibung 
         Caption         =   "1111"
      End
      Begin VB.Menu mnuWichtig 
         Caption         =   "1115"
      End
      Begin VB.Menu mnuHelpBar0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuHelpAbout 
         Caption         =   "1116"
      End
   End
   Begin VB.Menu Antennen 
      Caption         =   "Antennen + Kabel Engabe"
   End
End
Attribute VB_Name = "frmMain1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'============================================
'Hauptfenster als MDI Formular welches das
'Hauptformular frm Main übernimmt
' 12.09.2004  hb9zs
'4.11.2015 Ausdruck schwarzer Balken am oberen Rand eliminiert
' Internetadressen dm2ble und uska geändert
'19.07.2023 Alle Formularaufrufe für Internetexploreer geändert und
 '    Startaufruf für Antnennen und Kabeldateneingabe

'============================================

Option Explicit

Private Declare Function OSWinHelp% Lib "user32" Alias "WinHelpA" (ByVal hwnd&, ByVal HelpFile$, ByVal wCommand%, dwData As Any)
Private Declare Function ShellExecuteA Lib "shell32.dll" (ByVal hwnd As Long, ByVal lpOperation As Long, ByVal lpFile As String, ByVal lpParameters As Long, ByVal lpDirectory As Long, ByVal nShowCmd As Long) As Long
Private ap As String       'Def Path der Datei für IE Explorer
Private Const SW_SHOWNORMAL As Long = 1

Const inch = 1440 'Twips per Inch

Private Sub Antennen_Click()

Dim Ergebnis
    Ergebnis = Shell("C:\Feldstaerke_509\antennendaten_eingabe.exe", 0)

End Sub

Private Sub MDIForm_Load()

'Unload frmLiz

SaveSetting App.Title, "Settings", "Lang", frmStart.lblT6.Caption
    Me.Left = GetSetting(App.Title, "Settings", "Main1Left", 1000)
    Me.Top = GetSetting(App.Title, "Settings", "Main1Top", 1000)
    Me.Width = GetSetting(App.Title, "Settings", "Main1Width", 6500)
    Me.Height = GetSetting(App.Title, "Settings", "Main1Height", 6500)

frmMain1.Caption = LoadResString(1117 + RS)

'Programmregistrierung eintragen
'If Mid(frmLiz.LBL1.Caption, 3, 6) = Mid(frmx.Text2.Text, 3, 6) Then
'    frmMain1.Caption = LoadResString(1117 + RS) & " " & frmLiz.txtS2.Text
'    fGrid1.Label1.Caption = ""
'Else
'    frmMain1.Caption = LoadResString(1117 + RS) & " " & "unregistered copy"
'    fGrid1.Label1.Caption = " unregistered copy"
'End If

mnuFile.Caption = LoadResString(1101 + RS)
'mnuFileClose.Caption = LoadResString(1102 + RS)
Druck_laden.Caption = LoadResString(1103 + RS)
Druckspeichern.Caption = LoadResString(1104 + RS)
löschen.Caption = LoadResString(1105 + RS)
mnuFileprint.Caption = LoadResString(1106 + RS)
PrintFormLoad.Caption = LoadResString(1107 + RS)
Ant_Aufbereiten.Caption = LoadResString(1108 + RS)
mnuFileExit.Caption = LoadResString(1109 + RS)
mnuHelp.Caption = LoadResString(1110 + RS)
mnu_Progr_Beschreibung.Caption = LoadResString(1111 + RS)
mnuWichtig.Caption = LoadResString(1115 + RS)
mnuHelpAbout.Caption = LoadResString(1116 + RS)
mnu_Atennenbib.Caption = LoadResString(1118 + RS)
'mnu_Kabeldaten.Caption = LoadResString(1120 + RS)
mnu_Antdaten_Print.Caption = LoadResString(1121 + RS)
mnu_Antenne.Caption = LoadResString(1131 + RS)
mnu_Wegleitung.Caption = LoadResString(1132 + RS)
mnu_Formelblatt.Caption = LoadResString(1133 + RS)
mnu_Glossar.Caption = LoadResString(1134 + RS)
mnu_standortblatt.Caption = LoadResString(1135 + RS)
mnu_Beilage1.Caption = LoadResString(1136 + RS)
mnu_Beilage5.Caption = LoadResString(1137 + RS)
mnu_Beilage7.Caption = LoadResString(1138 + RS)
mnu_NISVerordnung.Caption = LoadResString(1139 + RS)
mnu_NISErlauterung.Caption = LoadResString(1140 + RS)
mnu_NISVerordnung1.Caption = LoadResString(1139 + RS)
mnu_NISErlauterung1.Caption = LoadResString(1140 + RS)
mnu_AntKommission.Caption = LoadResString(1119 + RS)

End Sub


Private Sub Ant_Aufbereiten_Click()

    'Antennen ZIP.exe File extrahieren und speichern.
    Dim Ergebnis
    Ergebnis = Shell(App.Path & "\ant_zip.exe", 0)

End Sub


Private Sub Druck_laden_Click()

' Gespeichertes Formular vom Disk laden
    fGrid1.lblGrid4.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub Druckspeichern_Click()

    'Speichern des Flex 1 (Druckformular)
    fGrid1.lblGrid3.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub löschen_Click()

    'Löschen Drucker Grid
    fGrid1.lblGrid5.Caption = Int(Rnd * 100) + 2
    
End Sub


Private Sub mnu_Antdaten_Print_Click()

    frmAntennenausdruck.Antennendaten_drucken

End Sub


Private Sub mnu_AntKommission_Click()
    
   On Error Resume Next
    
    If RS = 0 Then
    CreateObject("Wscript.Shell").Run "http://www.uska.ch/mitgliederservice/antennenkommission"
    End If
    
    If RS = 1000 Then
    CreateObject("Wscript.Shell").Run "http://www.uska.ch/typo/index.php?id=51&L=1"
    End If
    
    If RS = 2000 Then
        CreateObject("Wscript.Shell").Run "http://www.uska.ch/typo/index.php?id=51&L=2"
     End If

End Sub


Private Sub mnu_Atennenbib_Click()

    CreateObject("Wscript.Shell").Run "http://dm2ble.dk7by.de/KW-Quads/quads.htm"
    
End Sub


Private Sub mnu_Beilage1_Click()

     Dim ap As String

    'deutsches Dokument laden
    If RS = 0 Then
           ap = "\pdf_formulare\beilage_1_igw_d.pdf"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        ap = "\pdf_formulare\beilage_1_igw_f.pdf"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        ap = "\pdf_formulare\beilage_1_igw_i.pdf"
    End If

    ShellExecuteA Me.hwnd, 0, App.Path & ap, 0, 0, SW_SHOWNORMAL

End Sub


Private Sub mnu_Beilage5_Click()

    Dim ap As String

    'deutsches Dokument laden
    If RS = 0 Then
        ap = "\pdf_formulare\beilage5_tech_angaben_d.doc"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        ap = "\pdf_formulare\beilage5_tech_angaben_f.doc"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        ap = "\pdf_formulare\beilage5_tech_angaben_i.doc"
    End If

    ShellExecuteA Me.hwnd, 0, App.Path & ap, 0, 0, SW_SHOWNORMAL

End Sub


Private Sub mnu_Beilage7_Click()

    Dim ap As String

    'deutsches Dokument laden
    If RS = 0 Then
        ap = "\pdf_formulare\beilage7_zus_angaben_d_rev_a.doc"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        ap = "\pdf_formulare\beilage7_zus_angaben_f_rev_a.doc"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        ap = "\pdf_formulare\beilage7_zus_angaben_i_rev_a.doc"
    End If

    ShellExecuteA Me.hwnd, 0, App.Path & ap, 0, 0, SW_SHOWNORMAL

End Sub


Private Sub mnu_Formelblatt_Click()

    Dim ap As String

    'deutsches Dokument laden
    If RS = 0 Then
        ap = "\pdf_formulare\formelblatt_d.pdf"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        ap = "\pdf_formulare\formelblatt_f.pdf"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        ap = "\pdf_formulare\formelblatt_i.pdf"
    End If

    ShellExecuteA Me.hwnd, 0, App.Path & ap, 0, 0, SW_SHOWNORMAL

End Sub


Private Sub mnu_Glossar_Click()

'folgt noch

End Sub


Private Sub mnu_NISErlauterung_Click()

    'Internetexploreer auf frmMain starten Antennenbibliothek
    On Error Resume Next
       
    'deutsches Dokument laden
    If RS = 0 Then
         CreateObject("Wscript.Shell").Run "http://www.bafu.admin.ch/elektrosmog/01100/01101/index.html?lang=de&download=NHzLpZig7t,lnp6I0NTU042l2Z6ln1acy4Zn4Z2qZpnO2Yuq2Z6gpJCDe4R7fGym162dpYbUzd,Gpd6emK2Oz9aGodetmqaN19XI2IdvoaCVZ,s-.pdf"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
         CreateObject("Wscript.Shell").Run "http://www.bafu.admin.ch/elektrosmog/01117/index.html?lang=fr&download=NHzLpZig7t,lnp6I0NTU042l2Z6ln1ae2IZn4Z2qZpnO2Yuq2Z6gpJCDe4R7fGym162dpYbUzd,Gpd6emK2Oz9aGodetmqaN19XI2IdvoaCVZ,s-.pdf"
   End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
         CreateObject("Wscript.Shell").Run "http://www.bafu.admin.ch/elektrosmog/01079/index.html?lang=it&download=NHzLpZig7t,lnp6I0NTU042l2Z6ln1ah2oZn4Z2qZpnO2Yuq2Z6gpJCDe4B5fGym162dpYbUzd,Gpd6emK2Oz9aGodetmqaN19XI2IdvoaCVZ,s-.pdf"
    End If

End Sub

Private Sub mnu_NISErlauterung1_Click()

    'Internetexploreer auf frmMain starten Antennenbibliothek
  
    On Error Resume Next
   
    'deutsches Dokument laden
    If RS = 0 Then
          CreateObject("Wscript.Shell").Run "http://www.bafu.admin.ch/elektrosmog/01100/01101/index.html?lang=de&download=NHzLpZig7t,lnp6I0NTU042l2Z6ln1acy4Zn4Z2qZpnO2Yuq2Z6gpJCDe4R7fGym162dpYbUzd,Gpd6emK2Oz9aGodetmqaN19XI2IdvoaCVZ,s-.pdf"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
          CreateObject("Wscript.Shell").Run "http://www.bafu.admin.ch/elektrosmog/01117/index.html?lang=fr&download=NHzLpZig7t,lnp6I0NTU042l2Z6ln1ae2IZn4Z2qZpnO2Yuq2Z6gpJCDe4R7fGym162dpYbUzd,Gpd6emK2Oz9aGodetmqaN19XI2IdvoaCVZ,s-.pdf"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
         CreateObject("Wscript.Shell").Run "http://www.bafu.admin.ch/elektrosmog/01079/index.html?lang=it&download=NHzLpZig7t,lnp6I0NTU042l2Z6ln1ah2oZn4Z2qZpnO2Yuq2Z6gpJCDe4B5fGym162dpYbUzd,Gpd6emK2Oz9aGodetmqaN19XI2IdvoaCVZ,s-.pdf"
    End If

End Sub

Private Sub mnu_Wegleitung_Click()

    Dim ap As String

    'deutsches Dokument laden
    If RS = 0 Then
        ap = "\pdf_formulare\wegleitung_d_rev_a.pdf"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        ap = "\pdf_formulare\wegleitung_f_rev_a.pdf"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        ap = "\pdf_formulare\wegleitung_i_rev_a.pdf"
    End If

    ShellExecuteA Me.hwnd, 0, App.Path & ap, 0, 0, SW_SHOWNORMAL

End Sub

Private Sub mnu_Antenne_Click()

    Dim ap As String

    'deutsches Dokument laden
    If RS = 0 Then
        ap = "\pdf_formulare\antenne_was_tun_d_rev_a.pdf"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        ap = "\pdf_formulare\antenne_was_tun_f_rev_a.pdf"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        ap = "\pdf_formulare\antenne_was_tun_i_rev_a.pdf"
    End If

    ShellExecuteA Me.hwnd, 0, App.Path & ap, 0, 0, SW_SHOWNORMAL

End Sub


Private Sub mnu_NISVerordnung_Click()

    'Internetexploreer auf frmMain starten Antennenbibliothek
  
    On Error Resume Next
   
    'deutsches Dokument laden
    If RS = 0 Then
          CreateObject("Wscript.Shell").Run "http://www.admin.ch/ch/d/sr/c814_710.html"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
          CreateObject("Wscript.Shell").Run "http://www.admin.ch/ch/f/rs/c814_710.html"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
          CreateObject("Wscript.Shell").Run "http://www.admin.ch/ch/i/rs/c814_710.html"
    End If

End Sub


Private Sub mnu_NISVerordnung1_Click()

    'Internetexploreer auf frmMain starten Antennenbibliothek
    On Error Resume Next
    
     'deutsches Dokument laden
    If RS = 0 Then
         CreateObject("Wscript.Shell").Run "http://www.admin.ch/ch/d/sr/c814_710.html"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
          CreateObject("Wscript.Shell").Run "http://www.admin.ch/ch/f/rs/c814_710.html"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
         CreateObject("Wscript.Shell").Run "http://www.admin.ch/ch/i/rs/c814_710.html"
    End If

End Sub


Private Sub mnu_Progr_Beschreibung_Click()

    On Error Resume Next

    'deutsches Dokument laden
    If RS = 0 Then
        CreateObject("Wscript.Shell").Run App.Path & "\beschreibung_503\text_d\beschreibung503_d.html"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        CreateObject("Wscript.Shell").Run App.Path & "\beschreibung_503\text_f\beschreibung503_f.html"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        CreateObject("Wscript.Shell").Run App.Path & "\beschreibung_503\text_i\beschreibung503_i.html"
    End If


End Sub

Private Sub mnu_standortblatt_Click()

    Dim ap As String

    'deutsches Dokument laden
    If RS = 0 Then
        ap = "\pdf_formulare\emissionserklaerung_d_rev_a.doc"
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        ap = "\pdf_formulare\emissionserklaerung_f_rev_a.doc"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        ap = "\pdf_formulare\emissionserklaerung_i_rev_a.doc"
    End If


    ShellExecuteA Me.hwnd, 0, App.Path & ap, 0, 0, SW_SHOWNORMAL



End Sub



Private Sub PrintFormLoad_Click()
    
    fGrid1.Show                 'Druckerformular öffnen
   
End Sub


Private Sub mnuWichtig_Click()

    If RS = 0 Then
        CreateObject("Wscript.Shell").Run App.Path & "\lizenz\wichtig.htm"
     End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        CreateObject("Wscript.Shell").Run App.Path & "\lizenz\important.htm"
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        CreateObject("Wscript.Shell").Run App.Path & "\lizenz\importante.htm"
    End If
    
End Sub

Private Sub mnuHelpAbout_Click()
         
        frmAbout.Show vbModal, Me
          
End Sub

Private Sub mnuFileExit_Click()

    'Klassenmodul entladen
    Set clsFGEdit = Nothing
    Set clsFGEdit2 = Nothing
        
        SaveSetting App.Title, "Angaben", "Name", frmMain.txt03.Text
        SaveSetting App.Title, "Angaben", "Strasse", frmMain.txt04.Text
        SaveSetting App.Title, "Angaben", "Ort", frmMain.txt05.Text


    'Formular entladen
    Dim i As Integer

    'close all sub forms
    For i = Forms.Count - 1 To 1 Step -1
        Unload Forms(i)
    Next
    
    If Me.WindowState <> vbMinimized Then
       SaveSetting App.Title, "Settings", "Main1Left", Me.Left
       SaveSetting App.Title, "Settings", "Main1Top", Me.Top
       SaveSetting App.Title, "Settings", "Main1Width", Me.Width
       SaveSetting App.Title, "Settings", "Main1Height", Me.Height
    End If

    
    Unload frmMain1
  
End Sub

Private Sub mnuFilePrint_Click()

    'Ausdruck von FGrid1 Arial
    fGrid1.lblGrid2.Caption = Int(Rnd * 100) + 2
    
End Sub


Private Sub mnuFileClose_Click()

  'Formular entladen
    Dim i As Integer

    'close all sub forms
    For i = Forms.Count - 1 To 1 Step -1
        Unload Forms(i)
    Next
    If Me.WindowState <> vbMinimized Then
       SaveSetting App.Title, "Settings", "Main1Left", Me.Left
       SaveSetting App.Title, "Settings", "Main1Top", Me.Top
       SaveSetting App.Title, "Settings", "Main1Width", Me.Width
       SaveSetting App.Title, "Settings", "Main1Height", Me.Height
    End If
        
    Unload frmMain1
 
 End Sub
 

Private Sub MDIForm_resize()

On Error Resume Next
   
'    If frmMain1.Height > 10900 Then Height = 10920
'    If frmMain1.Width > 10650 Then Width = 10610
    If frmMain1.Height > 11470 Then frmMain1.Height = 11450 '11470 Then frmMain1.Height = 11450
    If frmMain1.Width > 11100 Then frmMain1.Width = 11090

End Sub

