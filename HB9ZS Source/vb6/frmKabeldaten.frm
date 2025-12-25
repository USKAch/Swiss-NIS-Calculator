VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Object = "{0ECD9B60-23AA-11D0-B351-00A0C9055D8E}#6.0#0"; "MSHFLXGD.OCX"
Begin VB.Form frmKabeldaten 
   Caption         =   "Kabeldaten-Eingabe"
   ClientHeight    =   8505
   ClientLeft      =   165
   ClientTop       =   855
   ClientWidth     =   6615
   LinkTopic       =   "Form1"
   ScaleHeight     =   8505
   ScaleWidth      =   6615
   StartUpPosition =   3  'Windows-Standard
   Begin MSComctlLib.StatusBar StatusBar1 
      Align           =   2  'Unten ausrichten
      Height          =   255
      Left            =   0
      TabIndex        =   53
      Top             =   8250
      Width           =   6615
      _ExtentX        =   11668
      _ExtentY        =   450
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   2
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            AutoSize        =   1
            Object.Width           =   9869
         EndProperty
         BeginProperty Panel2 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Style           =   5
            Object.Width           =   1235
            MinWidth        =   1235
            TextSave        =   "09:55"
         EndProperty
      EndProperty
   End
   Begin VB.TextBox TextBoxK1 
      Height          =   285
      Left            =   4680
      TabIndex        =   50
      Top             =   6240
      Visible         =   0   'False
      Width           =   975
   End
   Begin MSHierarchicalFlexGridLib.MSHFlexGrid FlexKabel1 
      Height          =   1095
      Left            =   120
      TabIndex        =   49
      Top             =   7440
      Visible         =   0   'False
      Width           =   5175
      _ExtentX        =   9128
      _ExtentY        =   1931
      _Version        =   393216
      _NumberOfBands  =   1
      _Band(0).Cols   =   2
   End
   Begin VB.Frame Frame1 
      Height          =   6855
      Left            =   600
      TabIndex        =   0
      Top             =   480
      Width           =   3975
      Begin VB.TextBox txt14 
         Height          =   285
         Left            =   1200
         TabIndex        =   30
         Text            =   "0"
         Top             =   6360
         Width           =   975
      End
      Begin VB.TextBox txt13 
         Height          =   285
         Left            =   1200
         TabIndex        =   29
         Text            =   "0"
         Top             =   6000
         Width           =   975
      End
      Begin VB.TextBox txt12 
         Height          =   285
         Left            =   1200
         TabIndex        =   28
         Text            =   "0"
         Top             =   5640
         Width           =   975
      End
      Begin VB.TextBox txt11 
         Height          =   285
         Left            =   1200
         TabIndex        =   27
         Text            =   "0"
         Top             =   5280
         Width           =   975
      End
      Begin VB.TextBox txt10 
         Height          =   285
         Left            =   1200
         TabIndex        =   26
         Text            =   "0"
         Top             =   4920
         Width           =   975
      End
      Begin VB.TextBox txt9 
         Height          =   285
         Left            =   1200
         TabIndex        =   25
         Text            =   "0"
         Top             =   4560
         Width           =   975
      End
      Begin VB.TextBox txt8 
         Height          =   285
         Left            =   1200
         TabIndex        =   24
         Text            =   "0"
         Top             =   4200
         Width           =   975
      End
      Begin VB.TextBox txt7 
         Height          =   285
         Left            =   1200
         TabIndex        =   23
         Text            =   "0"
         Top             =   3840
         Width           =   975
      End
      Begin VB.TextBox txt6 
         Height          =   285
         Left            =   1200
         TabIndex        =   22
         Text            =   "0"
         Top             =   3480
         Width           =   975
      End
      Begin VB.TextBox txt5 
         Height          =   285
         Left            =   1200
         TabIndex        =   21
         Text            =   "0"
         Top             =   3120
         Width           =   975
      End
      Begin VB.TextBox txt4 
         Height          =   285
         Left            =   1200
         TabIndex        =   20
         Text            =   "0"
         Top             =   2760
         Width           =   975
      End
      Begin VB.TextBox txt3 
         Height          =   285
         Left            =   1200
         TabIndex        =   19
         Text            =   "0"
         Top             =   2400
         Width           =   975
      End
      Begin VB.TextBox txt2 
         Height          =   285
         Left            =   1200
         TabIndex        =   18
         Text            =   "0"
         Top             =   2040
         Width           =   975
      End
      Begin VB.TextBox txt1 
         Height          =   285
         Left            =   1200
         TabIndex        =   17
         Text            =   "0"
         Top             =   1680
         Width           =   975
      End
      Begin VB.TextBox txt21 
         Height          =   285
         Left            =   2400
         TabIndex        =   16
         Text            =   "0"
         Top             =   1680
         Width           =   975
      End
      Begin VB.TextBox txt22 
         Height          =   285
         Left            =   2400
         TabIndex        =   15
         Text            =   "0"
         Top             =   2040
         Width           =   975
      End
      Begin VB.TextBox txt23 
         Height          =   285
         Left            =   2400
         TabIndex        =   14
         Text            =   "0"
         Top             =   2400
         Width           =   975
      End
      Begin VB.TextBox txt24 
         Height          =   285
         Left            =   2400
         TabIndex        =   13
         Text            =   "0"
         Top             =   2760
         Width           =   975
      End
      Begin VB.TextBox txt25 
         Height          =   285
         Left            =   2400
         TabIndex        =   12
         Text            =   "0"
         Top             =   3120
         Width           =   975
      End
      Begin VB.TextBox txt26 
         Height          =   285
         Left            =   2400
         TabIndex        =   11
         Text            =   "0"
         Top             =   3480
         Width           =   975
      End
      Begin VB.TextBox txt27 
         Height          =   285
         Left            =   2400
         TabIndex        =   10
         Text            =   "0"
         Top             =   3840
         Width           =   975
      End
      Begin VB.TextBox txt28 
         Height          =   285
         Left            =   2400
         TabIndex        =   9
         Text            =   "0"
         Top             =   4200
         Width           =   975
      End
      Begin VB.TextBox txt29 
         Height          =   285
         Left            =   2400
         TabIndex        =   8
         Text            =   "0"
         Top             =   4560
         Width           =   975
      End
      Begin VB.TextBox txt30 
         Height          =   285
         Left            =   2400
         TabIndex        =   7
         Text            =   "0"
         Top             =   4920
         Width           =   975
      End
      Begin VB.TextBox txt31 
         Height          =   285
         Left            =   2400
         TabIndex        =   6
         Text            =   "0"
         Top             =   5280
         Width           =   975
      End
      Begin VB.TextBox txt32 
         Height          =   285
         Left            =   2400
         TabIndex        =   5
         Text            =   "0"
         Top             =   5640
         Width           =   975
      End
      Begin VB.TextBox txt33 
         Height          =   285
         Left            =   2400
         TabIndex        =   4
         Text            =   "0"
         Top             =   6000
         Width           =   975
      End
      Begin VB.TextBox txt34 
         Height          =   285
         Left            =   2400
         TabIndex        =   3
         Text            =   "0"
         Top             =   6360
         Width           =   975
      End
      Begin VB.TextBox txt41 
         Height          =   285
         Left            =   1200
         TabIndex        =   2
         Top             =   720
         Width           =   975
      End
      Begin VB.TextBox txt42 
         Height          =   285
         Left            =   2400
         TabIndex        =   1
         Top             =   720
         Width           =   975
      End
      Begin VB.Label lbl2 
         Caption         =   "Eigen Typ 2"
         Height          =   495
         Left            =   2400
         TabIndex        =   52
         Top             =   240
         Width           =   975
      End
      Begin VB.Label lbl14 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   47
         Top             =   6405
         Width           =   855
      End
      Begin VB.Label lbl13 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   46
         Top             =   6075
         Width           =   855
      End
      Begin VB.Label lbl12 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   45
         Top             =   5685
         Width           =   855
      End
      Begin VB.Label lbl11 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   44
         Top             =   5325
         Width           =   855
      End
      Begin VB.Label lbl10 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   43
         Top             =   4965
         Width           =   855
      End
      Begin VB.Label lbl09 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   42
         Top             =   4605
         Width           =   855
      End
      Begin VB.Label lbl08 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   41
         Top             =   4275
         Width           =   855
      End
      Begin VB.Label lbl07 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   40
         Top             =   3885
         Width           =   855
      End
      Begin VB.Label lbl06 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   39
         Top             =   3525
         Width           =   855
      End
      Begin VB.Label lbl05 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   38
         Top             =   3165
         Width           =   855
      End
      Begin VB.Label lbl04 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   37
         Top             =   2805
         Width           =   855
      End
      Begin VB.Label lbl03 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   36
         Top             =   2445
         Width           =   855
      End
      Begin VB.Label lbl02 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   35
         Top             =   2055
         Width           =   855
      End
      Begin VB.Label lbl01 
         Alignment       =   1  'Rechts
         Caption         =   "Label1"
         Height          =   255
         Left            =   240
         TabIndex        =   34
         Top             =   1725
         Width           =   855
      End
      Begin VB.Label lbl21 
         Caption         =   "Kabeltyp des Herstellers"
         Height          =   375
         Left            =   80
         TabIndex        =   33
         Top             =   660
         Width           =   1095
      End
      Begin VB.Label lbl1 
         Caption         =   "Eigen Typ 1"
         Height          =   495
         Left            =   1200
         TabIndex        =   32
         Top             =   240
         Width           =   975
      End
      Begin VB.Label lbl3 
         Caption         =   "Kabeldämpfung in dB bezogen auf 100 m Kabellänge"
         Height          =   390
         Left            =   1200
         TabIndex        =   31
         Top             =   1245
         Width           =   2295
         WordWrap        =   -1  'True
      End
   End
   Begin VB.Label lblK2 
      Caption         =   "Kabeldaten"
      Height          =   255
      Left            =   4680
      TabIndex        =   51
      Top             =   6720
      Visible         =   0   'False
      Width           =   855
   End
   Begin VB.Label lbl20 
      Caption         =   "Eingabemaske für Kabeldaten die nicht in der Liste vorhanden sind."
      Height          =   255
      Left            =   360
      TabIndex        =   48
      Top             =   240
      Width           =   4815
      WordWrap        =   -1  'True
   End
   Begin VB.Menu mnu_Datei 
      Caption         =   "Datei"
      Begin VB.Menu mnu_LoadKabel 
         Caption         =   "Kabeldaten laden"
      End
      Begin VB.Menu mnu_Kabelspeichern 
         Caption         =   "Kabeldaten speichern"
      End
      Begin VB.Menu mnuFilebar1 
         Caption         =   "-"
      End
      Begin VB.Menu mnu_Beenden 
         Caption         =   "Beenden"
      End
   End
End
Attribute VB_Name = "frmKabeldaten"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'===============================================
'Formular zur Eingabe von Kabeltypen
'
'
'
'9.03.2004 / hb9zs
'================================================

Private yCol As Integer  'Kolonnenzahl des Kabelflexgrides


Public Sub SelectAll(TxtBox As TextBox)

'Einfügemarke für Text in den Eingabefeldern
    With TxtBox
        .SelStart = 0
        .SelLength = Len(.Text)
    End With
    
End Sub


Private Sub Form_Load()
    
    'Laden der Einstellungen aus Registry
    Me.Left = GetSetting(App.Title, "Settings", "KabelLeft", 1000)
    Me.Top = GetSetting(App.Title, "Settings", "KabelTop", 1000)
    Me.Width = GetSetting(App.Title, "Settings", "KabelWidth", 5500)
    Me.Height = GetSetting(App.Title, "Settings", "KabelHeight", 8490)
    
    frmKabeldaten.Caption = LoadResString(1771 + RS)
    mnu_Datei.Caption = LoadResString(1101 + RS)
    mnu_LoadKabel.Caption = LoadResString(1779 + RS)
    mnu_Kabelspeichern.Caption = LoadResString(1772 + RS)
    mnu_Beenden.Caption = LoadResString(1773 + RS)

    lbl20.Caption = LoadResString(1774 + RS)
    lbl21.Caption = LoadResString(1775 + RS)
    lbl1.Caption = LoadResString(1776 + RS)
    lbl2.Caption = LoadResString(1777 + RS)
    lbl3.Caption = LoadResString(1778 + RS)
    
    lblK2.Caption = "Kabelxdaten1"   'Dateiname festlegen
    FlexKabel1.Rows = 15

    DoKabeldaten_load               '"Kabeldaten1" vom Disk laden
    refreshform                     ' FlexKabel1 in Liste übertragen

End Sub


Private Sub Form_Resize()

    On Error Resume Next
    
    If frmKabeldaten.Height > 8490 Then
        frmKabeldaten.Height = 8490
    End If
    
    If frmKabeldaten.ScaleWidth > 5500 Then
        frmKabeldaten.Width = 5500
    End If
     
End Sub


Sub refreshform()

'Daten aus Flexgrid holen

    Dim yc1 As Integer
    Dim yc2 As Integer

    yc1 = yCol - 2  '1 Kolonne eigene Kabeldaten
    yc2 = yCol - 1  '2 Kolonne eigene Kabeldaten
    
    lbl01.Caption = "   1.8 MHz"
    lbl02.Caption = "   3.5 MHz"
    lbl03.Caption = "   7   MHz"
    lbl04.Caption = "  10   MHz"
    lbl05.Caption = "  14   MHz"
    lbl06.Caption = "  18   MHz"
    lbl07.Caption = "  21   MHz"
    lbl08.Caption = "  24   MHz"
    lbl09.Caption = "  28   MHz"
    lbl10.Caption = "  50   MHz"
    lbl11.Caption = " 144   MHz"
    lbl12.Caption = " 430   MHz"
    lbl13.Caption = "1296   MHz"
    lbl14.Caption = "2400   MHz"

    txt41.Text = FlexKabel1.TextMatrix(0, yc1)
    txt42.Text = FlexKabel1.TextMatrix(0, yc2)
    
    txt1.Text = FlexKabel1.TextMatrix(1, yc1)
    txt2.Text = FlexKabel1.TextMatrix(2, yc1)
    txt3.Text = FlexKabel1.TextMatrix(3, yc1)
    txt4.Text = FlexKabel1.TextMatrix(4, yc1)
    txt5.Text = FlexKabel1.TextMatrix(5, yc1)
    txt6.Text = FlexKabel1.TextMatrix(6, yc1)
    txt7.Text = FlexKabel1.TextMatrix(7, yc1)
    txt8.Text = FlexKabel1.TextMatrix(8, yc1)
    txt9.Text = FlexKabel1.TextMatrix(9, yc1)
    txt10.Text = FlexKabel1.TextMatrix(10, yc1)
    txt11.Text = FlexKabel1.TextMatrix(11, yc1)
    txt12.Text = FlexKabel1.TextMatrix(12, yc1)
    txt13.Text = FlexKabel1.TextMatrix(13, yc1)
    txt14.Text = FlexKabel1.TextMatrix(14, yc1)

    txt21.Text = FlexKabel1.TextMatrix(1, yc2)
    txt22.Text = FlexKabel1.TextMatrix(2, yc2)
    txt23.Text = FlexKabel1.TextMatrix(3, yc2)
    txt24.Text = FlexKabel1.TextMatrix(4, yc2)
    txt25.Text = FlexKabel1.TextMatrix(5, yc2)
    txt26.Text = FlexKabel1.TextMatrix(6, yc2)
    txt27.Text = FlexKabel1.TextMatrix(7, yc2)
    txt28.Text = FlexKabel1.TextMatrix(8, yc2)
    txt29.Text = FlexKabel1.TextMatrix(9, yc2)
    txt30.Text = FlexKabel1.TextMatrix(10, yc2)
    txt31.Text = FlexKabel1.TextMatrix(11, yc2)
    txt32.Text = FlexKabel1.TextMatrix(12, yc2)
    txt33.Text = FlexKabel1.TextMatrix(13, yc2)
    txt34.Text = FlexKabel1.TextMatrix(14, yc2)

End Sub


Private Sub mnu_Beenden_Click()
    
    Unload Me
    
End Sub


Private Sub Form_Unload(Cancel As Integer)
    
    If Me.WindowState <> vbMinimized Then
        SaveSetting App.Title, "Settings", "KabelLeft", Me.Left
        SaveSetting App.Title, "Settings", "KabelTop", Me.Top
        SaveSetting App.Title, "Settings", "KabelWidth", Me.Width
        SaveSetting App.Title, "Settings", "KabelHeight", Me.Height
    End If

End Sub


Private Sub mnu_Kabelspeichern_Click()

    'Daten auf Disk speichern
    lblK2.Caption = "Kabelxdaten1"  'Dateiname
    Datenübertragen
    'Kabeldaten_save
    Save_File_neu
    
End Sub

Private Sub mnu_LoadKabel_Click()

    DoKabeldaten_load  'Kabel Datei laden
    refreshform
    
End Sub

Private Sub txt21_GotFocus()
    
    SelectAll txt21

End Sub

Private Sub txt22_GotFocus()
    
    SelectAll txt22

End Sub

Private Sub txt23_GotFocus()
    
    SelectAll txt23

End Sub


Private Sub txt24_GotFocus()
    
    SelectAll txt24

End Sub


Private Sub txt25_GotFocus()
    
    SelectAll txt25

End Sub


Private Sub txt26_GotFocus()
    
    SelectAll txt26

End Sub


Private Sub txt27_GotFocus()
    
    SelectAll txt27

End Sub


Private Sub txt28_GotFocus()
    
    SelectAll txt28

End Sub


Private Sub txt29_GotFocus()
    
    SelectAll txt29

End Sub


Private Sub txt30_GotFocus()
    
    SelectAll txt30

End Sub


Private Sub txt31_GotFocus()
    
    SelectAll txt31

End Sub


Private Sub txt32_GotFocus()
    
    SelectAll txt32

End Sub


Private Sub txt33_GotFocus()
    
    SelectAll txt33

End Sub


Private Sub txt34_GotFocus()
    
    SelectAll txt34

End Sub
'==

Private Sub txt1_GotFocus()
    
    SelectAll txt1

End Sub

Private Sub txt2_GotFocus()
    
    SelectAll txt2

End Sub

Private Sub txt3_GotFocus()
    
    SelectAll txt3

End Sub


Private Sub txt4_GotFocus()
    
    SelectAll txt4

End Sub


Private Sub txt5_GotFocus()
    
    SelectAll txt5

End Sub


Private Sub txt6_GotFocus()
    
    SelectAll txt6

End Sub


Private Sub txt7_GotFocus()
    
    SelectAll txt7

End Sub


Private Sub txt8_GotFocus()
    
    SelectAll txt8

End Sub


Private Sub txt9_GotFocus()
    
    SelectAll txt9

End Sub


Private Sub txt10_GotFocus()
    
    SelectAll txt10

End Sub


Private Sub txt11_GotFocus()
    
    SelectAll txt11

End Sub


Private Sub txt12_GotFocus()
    
    SelectAll txt12

End Sub


Private Sub txt13_GotFocus()
    
    SelectAll txt13

End Sub


Private Sub txt14_GotFocus()
    
    SelectAll txt14

End Sub

'===

'Verhinderung der Fehleingaben ausser Zahlen und
'Punkt, Komma wird in Punkt umgewandelt
'
Private Sub txt1_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt1.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt2_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt2.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt3_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt3.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt4_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt4.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt5_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt5.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt6_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt6.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt7_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt7.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt8_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt8.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt9_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt9.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub

Private Sub txt10_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt10.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt11_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt11.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt12_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt12.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt13_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt13.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub

Private Sub txt14_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt14.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub

'===
'
Private Sub txt21_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt21.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt22_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt22.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt23_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt23.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt24_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt24.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt25_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt25.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt26_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt26.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt27_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt27.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt28_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt28.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt29_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt29.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub

Private Sub txt30_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt30.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt31_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt31.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt32_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt32.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txt33_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt33.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub

Private Sub txt34_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txt34.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Public Sub Datenübertragen()

'Daten in Flexgrid schreiben

    Dim yc1 As Integer
    Dim yc2 As Integer

    yc1 = yCol - 2  '1 Kolonne eigene Kabeldaten
    yc2 = yCol - 1  '2 Kolonne eigene Kabeldaten
    
    FlexKabel1.TextMatrix(0, yc1) = txt41.Text
    FlexKabel1.TextMatrix(0, yc2) = txt42.Text
    
    FlexKabel1.TextMatrix(1, yc1) = txt1.Text
    FlexKabel1.TextMatrix(2, yc1) = txt2.Text
    FlexKabel1.TextMatrix(3, yc1) = txt3.Text
    FlexKabel1.TextMatrix(4, yc1) = txt4.Text
    FlexKabel1.TextMatrix(5, yc1) = txt5.Text
    FlexKabel1.TextMatrix(6, yc1) = txt6.Text
    FlexKabel1.TextMatrix(7, yc1) = txt7.Text
    FlexKabel1.TextMatrix(8, yc1) = txt8.Text
    FlexKabel1.TextMatrix(9, yc1) = txt9.Text
    FlexKabel1.TextMatrix(10, yc1) = txt10.Text
    FlexKabel1.TextMatrix(11, yc1) = txt11.Text
    FlexKabel1.TextMatrix(12, yc1) = txt12.Text
    FlexKabel1.TextMatrix(13, yc1) = txt13.Text
    FlexKabel1.TextMatrix(14, yc1) = txt14.Text

    FlexKabel1.TextMatrix(1, yc2) = txt21.Text
    FlexKabel1.TextMatrix(2, yc2) = txt22.Text
    FlexKabel1.TextMatrix(3, yc2) = txt23.Text
    FlexKabel1.TextMatrix(4, yc2) = txt24.Text
    FlexKabel1.TextMatrix(5, yc2) = txt25.Text
    FlexKabel1.TextMatrix(6, yc2) = txt26.Text
    FlexKabel1.TextMatrix(7, yc2) = txt27.Text
    FlexKabel1.TextMatrix(8, yc2) = txt28.Text
    FlexKabel1.TextMatrix(9, yc2) = txt29.Text
    FlexKabel1.TextMatrix(10, yc2) = txt30.Text
    FlexKabel1.TextMatrix(11, yc2) = txt31.Text
    FlexKabel1.TextMatrix(12, yc2) = txt32.Text
    FlexKabel1.TextMatrix(13, yc2) = txt33.Text
    FlexKabel1.TextMatrix(14, yc2) = txt34.Text


 '   DoKabeldatenuebertragen

 '   frmMain.cboK31.ListIndex = 8    'auf ersten Eintrag setzen
 '   frmMain.cboK131.ListIndex = 8    'auf ersten Eintrag setzen


End Sub


'==== neue Kabeldaten =====

Private Sub DoKabeldaten_load()


'File mit Kolonnenangabe vom Disk laden

On Error Resume Next
            
     
    TextBoxK1.Text = ""   ' Textbox leeren
    
    Dim Text1
        
    Open App.Path & "\" & "Ant_Dat" & "\" & lblK2.Caption & ".dat" For Input As #1   ' Datei zum Einlesen öffnen.
    
    Do While Not EOF(1)   ' Schleife bis Dateiende.
        Input #1, Text1 ' Daten in zwei Variablen einlesen.
        TextBoxK1.Text = TextBoxK1.Text + Text1 + vbCrLf
    Loop

    Close #1   ' Datei schließen
   
  'Tabelle löschen
        FlexKabel1.Clear
           
  'Ausfiltern der Kolonnenanzahl
        Dim längezeilenT As String
    
  'ermittelt die die ersten 4 Zeichen des Datensatzes
        längezeilenT = Left(TextBoxK1.Text, 3)
  'nimmt die letzen 2 Zeichen
    längezeilenT = Right(längezeilenT, 2)
     
  'schreibt den markierten String als Zeilenanzahl in die
  'MSFlexGrid Eigenschaft Rows!
    FlexKabel1.Cols = längezeilenT + 1    '+ 2
    yCol = längezeilenT  'Kolonnenzahl für alle Kabeldaten einträge festlegen
 
  'Sperrt das Neuzeichnen des FlexTabelle.
    FlexKabel1.Redraw = False
    
  ' ermittelt die Anzahlt der Zeilen und Spalten.
  ' ermittelt die Strings, welche zwischen den doppelpunkten(:)liegen,
  ' und schreibt die Strings in jede einzelne Zelle.
    Dim XT As Long           ' Variable für die Spalten
    Dim YT As Long
    Dim StartT As Long       'Variable für den Markierungsanfang
    Dim EndeT As Long        'Variable für das Markierungsende
    EndeT = 1
    
    For YT = 0 To FlexKabel1.Rows - 1                           'ermittelt die Anzahlt der Zeilen
        
        For XT = 0 To FlexKabel1.Cols - 1                       'ermittelt die Anzahlt der Spalten
            
                    
            StartT = InStr(EndeT, TextBoxK1.Text, ":", 0)      'Sucht nach dem Simikolon und gibt die Position in einer Zahl aus,
            TextBoxK1.SelStart = StartT                        'Markierungsanfang setzen
            EndeT = InStr(StartT + 1, TextBoxK1.Text, ":", 0)  'Markierungsende errechnen
            TextBoxK1.SelLength = EndeT - 1 - StartT           'Markierungsende setzen.
        
                    
           'ändert die aktuellen Zelle entsprechend der For-Next-Umläufe
            FlexKabel1.Col = XT
            FlexKabel1.Row = YT

           'schreibt den selektierten Text in die aktive Zelle
           
            FlexKabel1.Text = TextBoxK1.SelText
            
           'ändert die aktuellen Zelle wieder entsprechend der For-Next-Umläufe,
           'um den Zustand vor der farbeänderung herzustellen:
            FlexKabel1.Col = XT
        
        Next XT
    
    Next YT

ErrHandler:
   If Not Err = cdlCancel Then Resume Next
            
     'Flexgrid auf  Kolonnen zurücksetzen
     FlexKabel1.Cols = FlexKabel1.Cols - 1
    
    'Aktiviert das Neuzeichnen des FlexTabelle.
    FlexKabel1.Redraw = True
    
 
End Sub


'======

Private Sub Save_File_neu()

'File mit Kolonnenangabe auf Disk speichern
    
 '   On Error Resume Next
    
  ' löscht den Inhalt der textbox
    TextBoxK1.Text = ""
    
    FlexKabel1.TextMatrix(0, 0) = FlexKabel1.Cols 'YT - 2  'Zeilenanzahl in Flexgrid eintragen
    
    FlexKabel1.TextMatrix(0, 0) = FlexKabel1.Cols
        
  ' Sperrt das Neuzeichnen des FlexTabelle.
    FlexKabel1.Redraw = False
    
  ' ermittelt die Anzahlt der Zeilen und Spalten und schreibt den Inhalt jeder Zelle
  ' von einem Doppelpunkt (:) getrennt, in die Textbox
  
   TextBoxK1.Text = ""
   
   Dim XTa As String
   Dim ZT As Long
   Dim XT As Long
   Dim YT As Long
   
   TextBoxK1.Text = ""   ' Textbox leeren
    
    For YT = 0 To FlexKabel1.Rows - 1
        
        For XT = 0 To FlexKabel1.Cols - 1
            FlexKabel1.Col = XT
            FlexKabel1.Row = YT
            TextBoxK1.Text = TextBoxK1.Text + ":" + FlexKabel1.Text
        Next XT
            'Zeilenumbruch
            TextBoxK1.Text = TextBoxK1.Text + ":" + Chr(13) + Chr(10)
        
    Next YT
    
  ' schreibt als letztes Zeichen in die Textbox ein Pipe-Zeichen (:)(|),
  ' ansonsten würde der Inhalt der letzten Zelle nicht ausgelesen werden,
  ' da der Inhalt nicht zwischen zwei Pipe-Zeichen stehen würde.
    TextBoxK1.Text = TextBoxK1.Text + ":"
    
  ' Aktiviert das Neuzeichnen des FlexTabelle.
    FlexKabel1.Redraw = True
   
    Open App.Path & "\" & "Ant_Dat" & "\" & lblK2.Caption & ".dat" For Output As #1   ' Datei zum Einlesen öffnen.
            Print #1, TextBoxK1.Text   ' Text in Datei schreiben.
    Close #1   ' Datei schließen.
     
ErrHandler:
    
   
End Sub

'===========================Ende===================================

