VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "ComDlg32.OCX"
Begin VB.Form frmStart 
   BackColor       =   &H8000000B&
   Caption         =   "Startseite"
   ClientHeight    =   6720
   ClientLeft      =   225
   ClientTop       =   870
   ClientWidth     =   7410
   Icon            =   "frmStart.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   ScaleHeight     =   6720
   ScaleWidth      =   7410
   StartUpPosition =   3  'Windows-Standard
   Begin VB.CommandButton cmdT5 
      Caption         =   "Start"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   5400
      MouseIcon       =   "frmStart.frx":0442
      MousePointer    =   99  'Benutzerdefiniert
      TabIndex        =   10
      Top             =   3360
      Width           =   1455
   End
   Begin VB.CommandButton cmdT8 
      Caption         =   "Testo Italiano"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   5400
      MouseIcon       =   "frmStart.frx":074C
      MousePointer    =   99  'Benutzerdefiniert
      TabIndex        =   9
      Top             =   1380
      Width           =   1455
   End
   Begin VB.CommandButton cmdT7 
      Caption         =   "Texte français"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   5400
      MouseIcon       =   "frmStart.frx":0A56
      MousePointer    =   99  'Benutzerdefiniert
      TabIndex        =   8
      Top             =   750
      Width           =   1455
   End
   Begin VB.CommandButton cmdT6 
      Caption         =   "Text deutsch"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   5400
      MouseIcon       =   "frmStart.frx":0D60
      MousePointer    =   99  'Benutzerdefiniert
      TabIndex        =   7
      Top             =   150
      Width           =   1455
   End
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   6720
      Top             =   2280
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'Kein
      Enabled         =   0   'False
      Height          =   4695
      Left            =   120
      Picture         =   "frmStart.frx":106A
      ScaleHeight     =   4695
      ScaleWidth      =   4935
      TabIndex        =   3
      Top             =   1920
      Width           =   4935
   End
   Begin VB.Label Label1 
      Height          =   855
      Left            =   5160
      TabIndex        =   12
      Top             =   4060
      Width           =   2100
   End
   Begin VB.Label lblT6 
      Caption         =   "0"
      Height          =   255
      Left            =   5880
      TabIndex        =   11
      Top             =   2400
      Visible         =   0   'False
      Width           =   495
   End
   Begin VB.Label lblT5 
      BackColor       =   &H8000000B&
      Caption         =   "Calcolo delle immissioni per i radioamatori in Svizzera"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   120
      TabIndex        =   6
      Top             =   1320
      Width           =   4455
   End
   Begin VB.Label lblVers 
      Caption         =   "Vers"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   5160
      TabIndex        =   5
      Top             =   5040
      Width           =   1455
   End
   Begin VB.Label lblT4 
      Caption         =   "Programm"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   5580
      TabIndex        =   4
      Top             =   2880
      Width           =   1095
   End
   Begin VB.Label lblT3 
      Caption         =   "Calcul des immissions pour les amateurs en Suisse"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   120
      TabIndex        =   2
      Top             =   720
      Width           =   4335
   End
   Begin VB.Label lblT2 
      Caption         =   $"frmStart.frx":4115
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1215
      Left            =   5160
      TabIndex        =   1
      Top             =   5400
      Width           =   2145
   End
   Begin VB.Label lblT1 
      Caption         =   "NIS Feldstärkeberechnungsprogramm für Funkamateue in der Schweiz"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4335
   End
   Begin VB.Menu mnu_datei 
      Caption         =   "1701"
      Begin VB.Menu mnu_antdat1 
         Caption         =   "1702"
      End
      Begin VB.Menu mnuWichtig 
         Caption         =   "Wichtig"
      End
      Begin VB.Menu mnu_space 
         Caption         =   "-"
      End
      Begin VB.Menu mnu_beenden1 
         Caption         =   "Beenden"
      End
   End
   Begin VB.Menu mnu_Hilfe 
      Caption         =   "?"
      Begin VB.Menu mnu_Beschreibung 
         Caption         =   "1111"
      End
   End
End
Attribute VB_Name = "frmStart"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'
'==================================================
'Start Fenster für das Programm mit Sprachauswahl
'Deutsch, Französisch
'Lizenzüberprüfung
'28. 09. 2003 hb9zs
'==================================================
'
Option Explicit

Private IEApp As Object    'Def für IE Explorer
Private ap As String       'Def Path der Datei für IE Explorer

Dim Pic As Picture  'Titelbild deklaration
 
 
Private Sub mnu_Beschreibung_Click()

'    On Error Resume Next
    'Starten des Internetexplorers
'    Dim IEApp As Object
'    Set IEApp = CreateObject("InternetExplorer.Application")

 '   IEApp.Visible = True

    'deutsches Dokument laden
    If RS = 0 Then
        CreateObject("Wscript.Shell").Run App.Path & "\beschreibung_503\text_d\beschreibung503_d.html"
    '    IEApp.Navigate ap
    End If
  
    'französisches Dokument laden
    If RS = 1000 Then
        CreateObject("Wscript.Shell").Run App.Path & "\beschreibung_503\text_f\beschreibung503_f.html"
     '   IEApp.Navigate ap
    End If
    
    'italienisches Dokument laden
    If RS = 2000 Then
        CreateObject("Wscript.Shell").Run App.Path & "\beschreibung_503\text_i\beschreibung503_i.html"
   '     IEApp.Navigate ap
    End If



End Sub

Private Sub Picture1_Paint()

    'Titelbild auf Grösse anpassen
    Picture1.Width = Picture1.ScaleWidth
    Picture1.Height = Picture1.ScaleHeight

    Set Pic = Picture1.Picture

    Dim PicWidth, PicHeight
  
    PicWidth = ScaleX(Pic.Width, vbHimetric, vbTwips)
    PicHeight = ScaleY(Pic.Height, vbHimetric, vbTwips)
    Picture1.PaintPicture Pic, 0, 0, Picture1.ScaleWidth, Picture1.ScaleHeight
  
End Sub

 
 Private Sub Form_Load()

    App.Title = "Feldstaerke"  'Registry Einträge werden unter Feldstärke abgelegt

    Picture1_Paint  '(Sub) Titelbild auf Grösse anpassen

    lblT6.Caption = GetSetting(App.Title, "Settings", "Lang")
    If lblT6.Caption = "" Then
    lblT6.Caption = 0
    End If
    
    RS = lblT6.Caption
        
    Color
    Titel
    Label1.Caption = LoadResString(1112 + RS)
    mnu_datei.Caption = LoadResString(1701 + RS)
    mnu_antdat1.Caption = LoadResString(1702 + RS)
    mnu_beenden1.Caption = LoadResString(1703 + RS)
'    mnu_Liz.Caption = LoadResString(1706 + RS)
'    mnu_Licence.Caption = LoadResString(1707 + RS)
    mnuWichtig.Caption = LoadResString(1708 + RS)
    mnu_Beschreibung.Caption = LoadResString(1111 + RS)

    'Anzeige der Lizenzinformation
'    frmx.LBL1.Caption = Int(Rnd * 100) + 2
    
    'Anzeige der Softwareversion
    lblVers.Caption = "Version " & App.Major & "." & App.Minor & "." & App.Revision
    Titel 'Sub

End Sub
 
 
Sub Titel()

    frmStart.Caption = LoadResString(1117 + RS)
'    If Mid(frmLiz.LBL1.Caption, 3, 6) = Mid(frmx.Text2.Text, 3, 6) Then
'        frmStart.Caption = LoadResString(1117 + RS) & " " & frmLiz.txtS2.Text
'    Else
'        frmStart.Caption = LoadResString(1117 + RS) & " " & "unregistred copy"
'    End If

End Sub

 
Sub Color()

    If lblT6.Caption = 0 Then
        lblT1.BackColor = vbYellow
        lblT3.BackColor = &H8000000B
        lblT5.BackColor = &H8000000B
    End If
    
    If lblT6.Caption = 1000 Then
        lblT1.BackColor = &H8000000B
        lblT3.BackColor = vbYellow
        lblT5.BackColor = &H8000000B
    End If
    
    If lblT6.Caption = 2000 Then
        lblT1.BackColor = &H8000000B
        lblT3.BackColor = &H8000000B
        lblT5.BackColor = vbYellow
    End If
    
End Sub

Private Sub cmdT5_Click()

'Start Programm
  '  frmx.LBL1.Caption = Int(Rnd * 100) + 2


  '  If Mid(frmLiz.LBL1.Caption, 3, 6) = Mid(frmx.Text2.Text, 3, 6) Then

        frmMain1.Show
        frmMain.Show
        Unload frmStart
'        Unload frmx
    
   ' Else
      
    '    frmLiz.Show
    '    frmMain1.Show   '==unterdrückung Lizerzierung
    '    frmMain.Show    '==unterdrückung Lizerzierung
    'End If

End Sub

Private Sub cmdT6_Click()

RS = 0  'deutsche Version
    mnu_datei.Caption = LoadResString(1701 + RS)
    mnu_antdat1.Caption = LoadResString(1702 + RS)
    mnu_beenden1.Caption = LoadResString(1703 + RS)
   ' mnu_Liz.Caption = LoadResString(1706 + RS)
   ' mnu_Licence.Caption = LoadResString(1707 + RS)
    mnuWichtig.Caption = LoadResString(1708 + RS)
    lblT6.Caption = RS
    
    Color 'Subprogramm
    Titel 'Subprogramm

End Sub

Private Sub cmdT7_Click()

RS = 1000  'französische Version
    mnu_datei.Caption = LoadResString(1701 + RS)
    mnu_antdat1.Caption = LoadResString(1702 + RS)
    mnu_beenden1.Caption = LoadResString(1703 + RS)
 '   mnu_Liz.Caption = LoadResString(1706 + RS)
 '   mnu_Licence.Caption = LoadResString(1707 + RS)
    mnuWichtig.Caption = LoadResString(1708 + RS)
    lblT6.Caption = RS
    
    Color 'Subprogramm
    Titel 'Subprogramm
    
End Sub

Private Sub cmdT8_Click()

RS = 2000  'italienische Version
    mnu_datei.Caption = LoadResString(1701 + RS)
    mnu_antdat1.Caption = LoadResString(1702 + RS)
    mnu_beenden1.Caption = LoadResString(1703 + RS)
  '  mnu_Liz.Caption = LoadResString(1706 + RS)
  '  mnu_Licence.Caption = LoadResString(1707 + RS)
    mnuWichtig.Caption = LoadResString(1708 + RS)
    lblT6.Caption = RS
    
    Color 'Subprogramm
    Titel 'Subprogramm
    
End Sub

Private Sub mnu_antdat1_Click()

'Antennen ZIP.exe File extrahieren und speichern.
    Dim Ergebnis As String
    Ergebnis = Shell(App.Path & "\ant_zip.exe", 0)

End Sub


Private Sub mnu_beenden1_Click()
    
    SaveSetting App.Title, "Settings", "Lang", lblT6.Caption
'    Unload frmx
'    Unload frmLiz
    Unload Me

End Sub


Private Sub mnu_Licence_Click()

 '   Unload frmLiz
 '   mnu_datei.Caption = LoadResString(1701 + RS)
 '   mnu_antdat1.Caption = LoadResString(1702 + RS)
 '   mnu_beenden1.Caption = LoadResString(1703 + RS)
 '   mnu_Liz.Caption = LoadResString(1706 + RS)
 '   mnu_Licence.Caption = LoadResString(1707 + RS)
 '   mnuWichtig.Caption = LoadResString(1708 + RS)
    
'   frmLiz.Show
        
End Sub


Private Sub mnuWichtig_Click()
    
 On Error Resume Next
    
    'deutsches Dokument laden
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
