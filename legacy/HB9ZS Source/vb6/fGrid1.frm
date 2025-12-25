VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.2#0"; "MSCOMCTL.OCX"
Object = "{3B7C8863-D78F-101B-B9B5-04021C009402}#1.2#0"; "RICHTX32.OCX"
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form fGrid1 
   Caption         =   "1608"
   ClientHeight    =   12510
   ClientLeft      =   285
   ClientTop       =   630
   ClientWidth     =   13830
   Icon            =   "fGrid1.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   ScaleHeight     =   12510
   ScaleWidth      =   13830
   Begin VB.Frame Frame3 
      BorderStyle     =   0  'Kein
      Height          =   255
      Left            =   13440
      TabIndex        =   18
      Top             =   10920
      Width           =   255
   End
   Begin MSComctlLib.StatusBar StatusBar1 
      Align           =   2  'Unten ausrichten
      Height          =   255
      Left            =   0
      TabIndex        =   11
      Top             =   12255
      Width           =   13830
      _ExtentX        =   24395
      _ExtentY        =   450
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   3
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            AutoSize        =   1
            Object.Width           =   18733
         EndProperty
         BeginProperty Panel2 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Style           =   6
            TextSave        =   "08.03.2020"
         EndProperty
         BeginProperty Panel3 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Style           =   5
            TextSave        =   "09:56"
         EndProperty
      EndProperty
   End
   Begin VB.Frame Frame1 
      BorderStyle     =   0  'Kein
      Height          =   12135
      Left            =   120
      TabIndex        =   0
      Top             =   0
      Width           =   13245
      Begin MSComctlLib.ProgressBar ProgressBar1 
         Height          =   255
         Left            =   3960
         TabIndex        =   17
         Top             =   4920
         Width           =   3975
         _ExtentX        =   7011
         _ExtentY        =   450
         _Version        =   393216
         Appearance      =   1
      End
      Begin VB.Frame Frame2 
         Height          =   1335
         Left            =   600
         TabIndex        =   15
         Top             =   0
         Width           =   7215
         Begin MSFlexGridLib.MSFlexGrid MSFlex0 
            Height          =   1095
            Left            =   -120
            TabIndex        =   16
            Top             =   120
            Width           =   7215
            _ExtentX        =   12726
            _ExtentY        =   1931
            _Version        =   393216
            AllowBigSelection=   0   'False
            Enabled         =   0   'False
            HighLight       =   0
            GridLines       =   0
            GridLinesFixed  =   0
            ScrollBars      =   0
            MergeCells      =   1
            BorderStyle     =   0
         End
      End
      Begin VB.HScrollBar HScroll2 
         Height          =   255
         LargeChange     =   500
         Left            =   0
         SmallChange     =   200
         TabIndex        =   10
         Top             =   11880
         Width           =   12735
      End
      Begin VB.VScrollBar VScroll2 
         Height          =   7935
         LargeChange     =   500
         Left            =   12480
         SmallChange     =   200
         TabIndex        =   9
         Top             =   240
         Width           =   255
      End
      Begin MSFlexGridLib.MSFlexGrid Flex1 
         Height          =   7215
         Left            =   600
         TabIndex        =   12
         Top             =   1320
         Width           =   11235
         _ExtentX        =   19817
         _ExtentY        =   12726
         _Version        =   393216
         Rows            =   4
         Cols            =   5
         FixedRows       =   0
         FixedCols       =   0
         AllowBigSelection=   0   'False
         Enabled         =   0   'False
         HighLight       =   0
         GridLinesFixed  =   1
         ScrollBars      =   0
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
      End
      Begin MSFlexGridLib.MSFlexGrid MSFlex2 
         Height          =   3375
         Left            =   240
         TabIndex        =   13
         Top             =   8760
         Width           =   9495
         _ExtentX        =   16748
         _ExtentY        =   5953
         _Version        =   393216
         BackColorFixed  =   -2147483643
         AllowBigSelection=   0   'False
         Enabled         =   0   'False
         HighLight       =   0
         GridLines       =   0
         GridLinesFixed  =   0
         ScrollBars      =   0
         MergeCells      =   1
         BorderStyle     =   0
      End
      Begin VB.Label Label2 
         Caption         =   "Label2"
         Height          =   255
         Left            =   9120
         TabIndex        =   20
         Top             =   120
         Visible         =   0   'False
         Width           =   255
      End
      Begin VB.Label lblGrid8 
         Height          =   255
         Left            =   10440
         TabIndex        =   19
         Top             =   120
         Visible         =   0   'False
         Width           =   615
      End
      Begin VB.Label Label1 
         Height          =   255
         Left            =   7680
         TabIndex        =   14
         Top             =   480
         Visible         =   0   'False
         Width           =   2415
      End
      Begin VB.Label lblGrid7 
         Caption         =   "0"
         BeginProperty DataFormat 
            Type            =   1
            Format          =   "0.0"
            HaveTrueFalseNull=   0
            FirstDayOfWeek  =   0
            FirstWeekOfYear =   0
            LCID            =   2055
            SubFormatType   =   1
         EndProperty
         Height          =   255
         Left            =   11520
         TabIndex        =   8
         Top             =   120
         Visible         =   0   'False
         Width           =   855
      End
      Begin VB.Label lblGrid6 
         Caption         =   "lblGrid6"
         Height          =   255
         Left            =   11520
         TabIndex        =   7
         Top             =   960
         Visible         =   0   'False
         Width           =   735
      End
      Begin VB.Label lblGrid5 
         Caption         =   "lblGrid5"
         Height          =   255
         Left            =   11400
         TabIndex        =   6
         Top             =   600
         Visible         =   0   'False
         Width           =   735
      End
      Begin VB.Label lblGrid2 
         Caption         =   "lblGrid2"
         Height          =   255
         Left            =   10560
         TabIndex        =   2
         Top             =   960
         Visible         =   0   'False
         Width           =   615
      End
      Begin VB.Label lblGrid1 
         Caption         =   "lblGrid1"
         Height          =   255
         Left            =   10560
         TabIndex        =   1
         Top             =   600
         Visible         =   0   'False
         Width           =   495
      End
   End
   Begin RichTextLib.RichTextBox RichTextBox1 
      Height          =   615
      Left            =   7320
      TabIndex        =   3
      Top             =   8160
      Visible         =   0   'False
      Width           =   1635
      _ExtentX        =   2884
      _ExtentY        =   1085
      _Version        =   393217
      Enabled         =   -1  'True
      TextRTF         =   $"fGrid1.frx":0442
   End
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   2520
      Top             =   8280
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Label lblGrid4 
      Caption         =   "lblGrid4"
      Height          =   255
      Left            =   6000
      TabIndex        =   5
      Top             =   8640
      Visible         =   0   'False
      Width           =   615
   End
   Begin VB.Label lblGrid3 
      Caption         =   "lblGrid3"
      Height          =   135
      Left            =   6120
      TabIndex        =   4
      Top             =   8280
      Visible         =   0   'False
      Width           =   615
   End
   Begin VB.Menu mnuflex 
      Caption         =   "1602"
      Begin VB.Menu laden 
         Caption         =   "1603"
      End
      Begin VB.Menu speichern 
         Caption         =   "1604"
      End
      Begin VB.Menu löschen 
         Caption         =   "1505"
         Enabled         =   0   'False
         Visible         =   0   'False
      End
      Begin VB.Menu Drucken 
         Caption         =   "1606"
      End
   End
End
Attribute VB_Name = "fGrid1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'================================================
'Formular Ausdruck. In diesem Formular werden die
'Berechnungsdaten für den Ausdruck aufbereitet
'
' 10.10.2003  hb9zs
'================================================

Option Explicit

Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
Private Declare Function HideCaret Lib "user32" (ByVal hwnd As Long) As Long
Private Declare Function ShowCaret Lib "user32" (ByVal hwnd As Long) As Long

Private Const WM_USER = &H400
'Schwarzer Balken
Private Const VP_FORMATRANGE = WM_USER + 125
'Private Const VP_FORMATRANGE = WM_USER + 100

Private Const VP_YESIDO = 456654

Dim Ascii As String



Private Sub Label1_Change()

    'Einntrag Programmregistrierung
    MSFlex0.TextMatrix(4, 1) = Label1.Caption

End Sub


Private Sub lblGrid2_change()

'    frmAusdruck.Show

    frmAusdruck.Formular_kopieren   'Sub

    frmAusdruck.Formular_drucken    'sub

End Sub



Private Sub Drucken_Click()

    'Zufallszahl für Ausdruck in lblGrid2
    lblGrid2.Caption = Int(Rnd * 100) + 2
    
End Sub

Private Sub laden_Click()

    'Formular laden
    lblGrid4.Caption = Int(Rnd * 100) + 2
    
End Sub


Private Sub speichern_Click()

    'Grid speichern
    lblGrid3.Caption = Int(Rnd * 100) + 2
    
End Sub

Private Sub löschen_Click()

    'Grid löschen
    lblGrid5.Caption = Int(Rnd * 100) + 2
    
End Sub


Private Sub lblGrid5_Change()


Dim c As Integer
Dim r As Integer

'====== Neu für Disk laden =====
    'FlexGrid 0  löschen
    For c = 1 To 5
        For r = 0 To 4
            ' Kolonne c löschen
            MSFlex0.TextMatrix(r, c) = " "
        Next r
    Next c

'======= Ende neu =============
    
    
    'FlexGrid löschen
    For c = 1 To 6
        For r = 0 To 23
            ' Kolonne c+2 löschen
            Flex1.TextMatrix(r, c + 2) = " "
        Next r
    Next c

    
'====== Neu für Disk laden =====
    'FlexGrid 2  löschen
    For c = 1 To 2
        For r = 0 To 32
            ' Kolonne c löschen
            MSFlex2.TextMatrix(r, c) = " "
        Next r
    Next c

'======= Ende neu =============
    
End Sub

Private Sub Scroll_Resize()

On Error Resume Next

'Scroll Einstellungen
     
     VScroll2.Move Width - 530, 0, 250, fGrid1.ScaleHeight - StatusBar1.Height - HScroll2.Height
     HScroll2.Move 0, fGrid1.ScaleHeight - StatusBar1.Height - HScroll2.Height, Width - 2 * (VScroll2.Width)

     VScroll2.Max = Frame2.Height + Flex1.Height + MSFlex2.Height - Height + 1700
'    HScroll2.Max = Flex1.Width + 200 - Width + 400 'für Flex1
     HScroll2.Max = MSFlex2.Width + 200 - Width + 400 'für MSFlex2

    'Frame 3 in rechter unterer Ecke einblenden, damit Schrift verdeckt wird
    Frame3.Top = VScroll2.Height
    Frame3.Left = HScroll2.Width + 130
    
    'immer in den Vordergrund stellen mit ZOrder
    Frame3.ZOrder (0)

    
    If fGrid1.Height > 13960 Then
        fGrid1.Height = 13960
    End If
    
    If fGrid1.Width > 10350 Then  '9700 Then
        fGrid1.Width = 10350    '9700
    End If
    
    If fGrid1.Width > 10350 Then '9600 Then
        HScroll2.Visible = False
    Else
        HScroll2.Visible = True
    End If
        
End Sub

Private Sub Form_Resize()

    Scroll_Resize
    
End Sub

Private Sub HScroll2_Change()

    Flex1.Left = -HScroll2.Value
    MSFlex2.Left = -HScroll2.Value
    'Unterdrücken des Scrollbar blinkens
    HideCaret HScroll2.hwnd

    
End Sub

Private Sub VScroll2_Change()
    
    Frame2.Top = -VScroll2.Value
    Flex1.Top = -VScroll2.Value - 300 + Frame2.Height + 400 '100
    MSFlex2.Top = -VScroll2.Value - 300 + Frame2.Height + Flex1.Height + 500 '200
    'Unterdrücken des Scrollbar blinkens
    HideCaret VScroll2.hwnd


End Sub


Private Sub VScroll2_GotFocus()
    
    'Unterdrücken des Scrollbar blinkens
    HideCaret VScroll2.hwnd

End Sub


Private Sub VScroll2_LostFocus()
    
    'Unterdrücken des Scrollbar blinkens
    HideCaret VScroll2.hwnd

End Sub


Private Sub HScroll2_GotFocus()
    
    'Unterdrücken des Scrollbar blinkens
    HideCaret HScroll2.hwnd

End Sub


Private Sub HScroll2_LostFocus()
    
    'Unterdrücken des Scrollbar blinkens
    HideCaret HScroll2.hwnd

End Sub


Private Sub Form_Unload(Cancel As Integer)
    
    If Me.WindowState <> vbMinimized Then
        SaveSetting App.Title, "Settings", "fGrid1Left", fGrid1.Left
        SaveSetting App.Title, "Settings", "fGrid1Top", fGrid1.Top
        SaveSetting App.Title, "Settings", "fGrid1Width", fGrid1.Width
        SaveSetting App.Title, "Settings", "fGrid1Height", fGrid1.Height
    End If

End Sub


Private Sub Form_Load()

    fGrid1.Left = GetSetting(App.Title, "Settings", "fGrid1Left", 1000)
    fGrid1.Top = GetSetting(App.Title, "Settings", "fGrid1Top", 1000)
    fGrid1.Width = GetSetting(App.Title, "Settings", "fGrid1Width", 6500)
    fGrid1.Height = GetSetting(App.Title, "Settings", "fGrid1Height", 6500)

      'Grösse des frm2  bestimmen (dienen zum ausblenden
    'der unteren rechten Frameecke
    Frame3.Height = HScroll2.Height
    Frame3.Width = VScroll2.Width


    Scroll_Resize
        Dim i   As Integer
     
        For i = 0 To 1
        Frame1 = ""
   
        Next i

    Dim k As Integer, t As Integer
    Dim Zeile As Variant, Spalte As Variant
    
    
'==== Flex 0  =========================


    'Kolonnen und Reihen festlegen
    MSFlex0.Cols = 6
    MSFlex0.Rows = 7 '5

    'Kolonnen Breite festlegen
    MSFlex0.ColWidth(0) = 0
    MSFlex0.ColWidth(1) = 3500
    MSFlex0.ColWidth(2) = 3200
    MSFlex0.ColWidth(3) = 650
    MSFlex0.ColWidth(4) = 350
    MSFlex0.ColWidth(5) = 650
    MSFlex0.Left = 0
    'linksbündige Kolonne 2
    MSFlex0.ColAlignment(2) = 2


    'Breite und Höhe vom Flexgrid festlegen
    Frame2.Left = 0
    Frame2.Height = 0 + MSFlex0.RowHeight(1) * 5 + 150
    Frame2.Width = 0 + MSFlex0.ColWidth(0) + MSFlex0.ColWidth(1) + MSFlex0.ColWidth(2) + MSFlex0.ColWidth(3) + MSFlex0.ColWidth(4) + MSFlex0.ColWidth(5) + 50
    MSFlex0.Width = 0 + MSFlex0.ColWidth(0) + MSFlex0.ColWidth(1) + MSFlex0.ColWidth(2) + MSFlex0.ColWidth(3) + MSFlex0.ColWidth(4) + MSFlex0.ColWidth(5)
    MSFlex0.Height = 0 + MSFlex0.RowHeight(1) * MSFlex0.Rows
    MSFlex0.BackColorFixed = vbWhite
    'Texte
    MSFlex0.TextMatrix(0, 1) = LoadResString(1601 + RS) & "   " '"Immissionsberechnung für: "
    
    
    MSFlex0.TextMatrix(2, 1) = LoadResString(1750 + RS)
    MSFlex0.TextMatrix(3, 1) = LoadResString(1751 + RS)

    MSFlex0.TextMatrix(2, 2) = LoadResString(1756 + RS)
    MSFlex0.TextMatrix(3, 2) = LoadResString(1752 + RS)
    MSFlex0.TextMatrix(4, 2) = LoadResString(1753 + RS)

    MSFlex0.TextMatrix(3, 3) = LoadResString(1754 + RS)
    MSFlex0.TextMatrix(4, 3) = LoadResString(1754 + RS)

    MSFlex0.TextMatrix(3, 5) = LoadResString(1755 + RS)
    MSFlex0.TextMatrix(4, 5) = LoadResString(1755 + RS)



'=========================================

'Flex 1 definieren

    'Flex1.Rows = 24
    Flex1.Rows = 29
    Flex1.Cols = 9
  
    
    Flex1.ColWidth(0) = 10
    Flex1.ColWidth(1) = 3300
    Flex1.ColWidth(2) = 650
    Flex1.ColWidth(3) = 600
    Flex1.ColWidth(4) = 900
    Flex1.ColWidth(5) = 900
    Flex1.ColWidth(6) = 900
    Flex1.ColWidth(7) = 900
    Flex1.ColWidth(8) = 900
    Flex1.Left = 0
    Flex1.Top = Frame2.Top + Frame2.Height + 100
    Flex1.Width = 100 + Flex1.ColWidth(0) + Flex1.ColWidth(1) + Flex1.ColWidth(2) + Flex1.ColWidth(3) + Flex1.ColWidth(4) + Flex1.ColWidth(5) + Flex1.ColWidth(6) + Flex1.ColWidth(7) + Flex1.ColWidth(8)
    Flex1.Height = 80 + Flex1.RowHeight(1) * 23  '28  Flex1.Rows

    
'Flex1 Definitionen
        Flex1.Row = Zeile
        Flex1.Col = Spalte
     
'Spalte 1 links ausrichten
    Flex1.ColAlignment(1) = 1
    
'Spalten 3-9 rechts ausrichten
        Dim A As Integer
        For A = 3 To 8
            Flex1.ColAlignment(A) = 7
        Next A
    
'Titelleiste Text eintragen
    fGrid1.Caption = LoadResString(1608 + RS)
    mnuflex.Caption = LoadResString(1602 + RS)
    laden.Caption = LoadResString(1603 + RS)
    speichern.Caption = LoadResString(1604 + RS)
    löschen.Caption = LoadResString(1605 + RS)
    Drucken.Caption = LoadResString(1606 + RS)

' Giternetz anzeigen
    Flex1.GridLines = 1
  
'Flexfelder Text eintragen

    Flex1.TextMatrix(0, 1) = LoadResString(1209 + RS)  'fett
    Flex1.TextMatrix(1, 1) = LoadResString(1709 + RS)
    Flex1.TextMatrix(2, 1) = LoadResString(1213 + RS)
    Flex1.TextMatrix(3, 1) = LoadResString(1217 + RS)
    Flex1.TextMatrix(27, 1) = LoadResString(1220 + RS)
    Flex1.TextMatrix(4, 1) = LoadResString(1221 + RS)
    Flex1.TextMatrix(5, 1) = LoadResString(1223 + RS)
    Flex1.TextMatrix(6, 1) = LoadResString(1225 + RS)
    Flex1.TextMatrix(7, 1) = LoadResString(1228 + RS)
    Flex1.TextMatrix(8, 1) = LoadResString(1231 + RS)
    Flex1.TextMatrix(9, 1) = LoadResString(1234 + RS)
    Flex1.TextMatrix(10, 1) = LoadResString(1237 + RS)
    Flex1.TextMatrix(11, 1) = LoadResString(1239 + RS)
    Flex1.TextMatrix(12, 1) = LoadResString(1242 + RS)
    Flex1.TextMatrix(13, 1) = LoadResString(1245 + RS)
    Flex1.TextMatrix(14, 1) = LoadResString(1248 + RS)
    Flex1.TextMatrix(15, 1) = LoadResString(1250 + RS)
    Flex1.TextMatrix(16, 1) = LoadResString(1253 + RS)
    Flex1.TextMatrix(17, 1) = LoadResString(1256 + RS)
    Flex1.TextMatrix(18, 1) = LoadResString(1259 + RS)
    Flex1.TextMatrix(19, 1) = LoadResString(1264 + RS)
    Flex1.TextMatrix(20, 1) = LoadResString(1266 + RS) 'fett
    Flex1.TextMatrix(21, 1) = LoadResString(1270 + RS)
    Flex1.TextMatrix(22, 1) = LoadResString(1273 + RS) 'fett
    
'Text Spalte 2 eintragen
    Flex1.TextMatrix(0, 2) = LoadResString(1210 + RS)
    Flex1.TextMatrix(1, 2) = "" 'LoadResString(1214 + RS)
    Flex1.TextMatrix(2, 2) = LoadResString(1214 + RS)
    Flex1.TextMatrix(3, 2) = LoadResString(1218 + RS)
    Flex1.TextMatrix(27, 2) = frmMain.lbl41.Caption
    Flex1.TextMatrix(4, 2) = LoadResString(1222 + RS)
    Flex1.TextMatrix(5, 2) = LoadResString(1224 + RS)
    Flex1.TextMatrix(6, 2) = LoadResString(1226 + RS)
    Flex1.TextMatrix(7, 2) = LoadResString(1229 + RS)
    Flex1.TextMatrix(8, 2) = LoadResString(1232 + RS)
    Flex1.TextMatrix(9, 2) = LoadResString(1235 + RS)
    Flex1.TextMatrix(10, 2) = LoadResString(1238 + RS)
    Flex1.TextMatrix(11, 2) = LoadResString(1240 + RS)
    Flex1.TextMatrix(12, 2) = LoadResString(1243 + RS)
    Flex1.TextMatrix(13, 2) = LoadResString(1246 + RS)
    Flex1.TextMatrix(14, 2) = LoadResString(1249 + RS)
    Flex1.TextMatrix(15, 2) = LoadResString(1251 + RS)
    Flex1.TextMatrix(16, 2) = LoadResString(1254 + RS)
    Flex1.TextMatrix(17, 2) = LoadResString(1257 + RS)
    Flex1.TextMatrix(18, 2) = LoadResString(1260 + RS)
    Flex1.TextMatrix(19, 2) = LoadResString(1265 + RS)
    Flex1.TextMatrix(20, 2) = LoadResString(1267 + RS)
    Flex1.TextMatrix(21, 2) = LoadResString(1271 + RS)
    Flex1.TextMatrix(22, 2) = LoadResString(1274 + RS)


    'Text Spalte 3 eintragen
    Flex1.TextMatrix(0, 3) = LoadResString(1211) ' + RS) 'fett
    Flex1.TextMatrix(2, 3) = LoadResString(1215) ' + RS)
    Flex1.TextMatrix(3, 3) = LoadResString(1219) ' + RS)
    Flex1.TextMatrix(4, 3) = "[ ]"
    Flex1.TextMatrix(5, 3) = "[ ]"
    Flex1.TextMatrix(6, 3) = LoadResString(1227) ' + RS)
    Flex1.TextMatrix(7, 3) = LoadResString(1230) ' + RS)
    Flex1.TextMatrix(8, 3) = LoadResString(1233) ' + RS)
    Flex1.TextMatrix(9, 3) = LoadResString(1236) ' + RS)
    Flex1.TextMatrix(10, 3) = "[ ]"
    Flex1.TextMatrix(11, 3) = LoadResString(1241) ' + RS)
    Flex1.TextMatrix(12, 3) = LoadResString(1244) ' + RS)
    Flex1.TextMatrix(13, 3) = LoadResString(1247) ' + RS)
    Flex1.TextMatrix(14, 3) = "[ ]"
    Flex1.TextMatrix(15, 3) = LoadResString(1252) ' + RS)
    Flex1.TextMatrix(16, 3) = LoadResString(1255) ' + RS)
    Flex1.TextMatrix(17, 3) = LoadResString(1258) ' + RS)
    Flex1.TextMatrix(18, 3) = "[ ]"
    Flex1.TextMatrix(19, 3) = "[ ]"
    Flex1.TextMatrix(20, 3) = LoadResString(1268) ' + RS) 'fett
    Flex1.TextMatrix(21, 3) = LoadResString(1272) ' + RS)
    Flex1.TextMatrix(22, 3) = LoadResString(1275) ' + RS) 'fett
    
    

    
    'Kolonne 4 gelb markieren
    For A = 0 To 23
    fGrid1.Flex1.Row = A
    fGrid1.Flex1.Col = 4
    fGrid1.Flex1.CellBackColor = &H80FFFF 'Gelb
    Next A
 
 '==== MSFlex 2 ==================================

    'Kolonnen und Reihen festlegen
    MSFlex2.Cols = 3
    MSFlex2.Rows = 33
    
    'Kolonnen Breite festlegen
    MSFlex2.ColWidth(0) = 0
    MSFlex2.ColWidth(1) = 3330
    MSFlex2.ColWidth(2) = 6500
    MSFlex2.Left = 0
    
    'linksbündige Kolonnen mit Zahlen
    MSFlex2.ColAlignment(1) = 2
    MSFlex2.ColAlignment(2) = 2

    'Breite und Höhe vom Flexgrid festlegen
    MSFlex2.Top = Frame2.Top + Frame2.Height + Flex1.Height + 200

    MSFlex2.Width = 0 + MSFlex2.ColWidth(0) + MSFlex2.ColWidth(1) + MSFlex2.ColWidth(2)
    MSFlex2.Height = 0 + MSFlex2.RowHeight(1) * MSFlex2.Rows
    'Texte
    MSFlex2.TextMatrix(0, 2) = LoadResString(1759 + RS) & " " & LoadResString(1760 + RS)
    
    MSFlex2.TextMatrix(2, 1) = LoadResString(1209 + RS) 'Frequenz
    MSFlex2.TextMatrix(2, 2) = LoadResString(1721 + RS)
    
    MSFlex2.TextMatrix(3, 1) = LoadResString(1709 + RS) 'Nr. des OK
    MSFlex2.TextMatrix(3, 2) = LoadResString(1710 + RS)
    
    MSFlex2.TextMatrix(4, 1) = LoadResString(1213 + RS) 'Abstand OK
    MSFlex2.TextMatrix(4, 2) = LoadResString(1722 + RS)
    MSFlex2.TextMatrix(5, 2) = LoadResString(1723 + RS)
    MSFlex2.TextMatrix(6, 2) = LoadResString(1724 + RS) '
    
    MSFlex2.TextMatrix(7, 1) = LoadResString(1217 + RS) 'Leistung am Senderausgang
    MSFlex2.TextMatrix(7, 2) = LoadResString(1700 + RS)
    
    MSFlex2.TextMatrix(8, 1) = LoadResString(1221 + RS)
    MSFlex2.TextMatrix(8, 2) = LoadResString(1726 + RS)
    MSFlex2.TextMatrix(9, 1) = LoadResString(1223 + RS)
    MSFlex2.TextMatrix(9, 2) = LoadResString(1727 + RS)
    MSFlex2.TextMatrix(10, 1) = LoadResString(1225 + RS)
    MSFlex2.TextMatrix(10, 2) = LoadResString(1728 + RS)
    MSFlex2.TextMatrix(11, 1) = LoadResString(1228 + RS)
    MSFlex2.TextMatrix(11, 2) = " "
    MSFlex2.TextMatrix(12, 2) = " "
    MSFlex2.TextMatrix(13, 2) = " "
    
    MSFlex2.TextMatrix(14, 1) = LoadResString(1231 + RS)

  
    MSFlex2.TextMatrix(16, 1) = LoadResString(1234 + RS)
    MSFlex2.TextMatrix(16, 2) = LoadResString(1729 + RS)
    MSFlex2.TextMatrix(17, 1) = LoadResString(1237 + RS)
    MSFlex2.TextMatrix(17, 2) = LoadResString(1730 + RS)
    MSFlex2.TextMatrix(18, 1) = LoadResString(1239 + RS)
    MSFlex2.TextMatrix(18, 2) = LoadResString(1731 + RS)
    MSFlex2.TextMatrix(19, 1) = LoadResString(1242 + RS)
    MSFlex2.TextMatrix(19, 2) = LoadResString(1732 + RS)
    MSFlex2.TextMatrix(20, 1) = LoadResString(1245 + RS)
    MSFlex2.TextMatrix(20, 2) = LoadResString(1733 + RS)
    MSFlex2.TextMatrix(21, 1) = LoadResString(1248 + RS)
    MSFlex2.TextMatrix(21, 2) = LoadResString(1734 + RS)
    MSFlex2.TextMatrix(22, 1) = LoadResString(1250 + RS)
    MSFlex2.TextMatrix(22, 2) = LoadResString(1735 + RS)
    MSFlex2.TextMatrix(23, 1) = LoadResString(1253 + RS)
    MSFlex2.TextMatrix(23, 2) = LoadResString(1736 + RS)
    MSFlex2.TextMatrix(24, 1) = LoadResString(1256 + RS)
    MSFlex2.TextMatrix(24, 2) = LoadResString(1737 + RS)
    MSFlex2.TextMatrix(25, 1) = LoadResString(1259 + RS)
    MSFlex2.TextMatrix(25, 2) = LoadResString(1738 + RS)
    
    
    MSFlex2.TextMatrix(26, 1) = LoadResString(1264 + RS)
    MSFlex2.TextMatrix(26, 2) = LoadResString(1740 + RS)
    MSFlex2.TextMatrix(27, 1) = LoadResString(1266 + RS)
    MSFlex2.TextMatrix(27, 2) = LoadResString(1741 + RS)
    MSFlex2.TextMatrix(28, 1) = LoadResString(1270 + RS)
    MSFlex2.TextMatrix(28, 2) = LoadResString(1743 + RS)
    MSFlex2.TextMatrix(29, 1) = LoadResString(1273 + RS)
    MSFlex2.TextMatrix(29, 2) = LoadResString(1744 + RS)
    MSFlex2.TextMatrix(30, 2) = LoadResString(1761 + RS)
    MSFlex2.TextMatrix(32, 1) = frmMain.txt05.Text & ", " & Date  'Ort und Datum
    
    MSFlex2.TextMatrix(32, 2) = LoadResString(1745 + RS)

    Fettschrift     'Sub

    Frame1.Height = MSFlex0.Height + Flex1.Height + MSFlex2.Height + 500
    Frame1.Width = MSFlex2.Width + 300 'Flex1.Width + 300

'??????    'Anzeige von Flex1 bei reg Kopien ===================
    
    
'    MSFlex0.TextMatrix(4, 1) = Label1.Caption


'??????

    ProgressBar1.Visible = False
    
    Form_Resize

End Sub
        
        
Private Sub Fettschrift()

    'Def. für Fettschrift
    Dim X As Integer
    Dim Y As Integer
    Dim Kolonne As Integer


    'MSFlex0 Fettschrift Kolonnen 1 und 2
    
    For Kolonne = 1 To 1
            'Zelle selektieren
            MSFlex0.Row = 0
            MSFlex0.Col = Kolonne
            MSFlex0.CellFontBold = True
    Next Kolonne
            MSFlex0.Row = 0
            MSFlex0.Col = 1
            MSFlex0.CellAlignment = 7
            
            MSFlex0.Row = 4
            MSFlex0.Col = 1
            MSFlex0.CellFontBold = True



    ' Flex1 Zeile 29 Fett drucken
    Dim Spalte As Integer
    For Spalte = 1 To 8
        Flex1.Row = 0
        Flex1.Col = Spalte
        Flex1.CellFontBold = True
    Next Spalte
    
    For Spalte = 1 To 8
        Flex1.Row = 1
        Flex1.Col = Spalte
        Flex1.CellFontBold = True
    Next Spalte
    
        For Spalte = 1 To 8
        Flex1.Row = 2
        Flex1.Col = Spalte
        Flex1.CellFontBold = True
    Next Spalte

    For Spalte = 1 To 8
        Flex1.Row = 3
        Flex1.Col = Spalte
        Flex1.CellFontBold = True
    Next Spalte

    For Spalte = 1 To 8
        Flex1.Row = 6
        Flex1.Col = Spalte
        Flex1.CellFontBold = True
    Next Spalte

    For Spalte = 1 To 8
        Flex1.Row = 16
        Flex1.Col = Spalte
        Flex1.CellFontBold = True
    Next Spalte

    
    For Spalte = 1 To 8
        Flex1.Row = 20 '21
        Flex1.Col = Spalte
        Flex1.CellFontBold = True
    Next Spalte
    
    For Spalte = 1 To 8
        Flex1.Row = 22 '23
        Flex1.Col = Spalte
        Flex1.CellFontBold = True
    Next Spalte


    ' Fettschrift MSFlex 2
    
    For X = 1 To 1  '6  nur erste Reihe
        If X = 1 Then
            Y = 0           'Reihe 0
        ElseIf X = 2 Then
            Y = 2           'Reihe 2
        ElseIf X = 3 Then
            Y = 26          'Reihe 25
        ElseIf X = 4 Then
            Y = 27          'Reihe 26
        ElseIf X = 5 Then
            Y = 29          'Reihe 28
        ElseIf X = 6 Then
            Y = 30          'Reihe 28
        End If
        
        
        'Fettschrift Kolonnen 1 und 2
        For Kolonne = 1 To 2
            'Zelle selektieren
            MSFlex2.Row = Y
            MSFlex2.Col = Kolonne
            MSFlex2.CellFontBold = True
        Next Kolonne

    Next X

End Sub

        
    
Private Sub lblGrid1_change()
     
 On Error Resume Next
 
'Eintragen der berechneten Werte in FlexGrid.
'Name und Adresse einfüllen
    MSFlex0.TextMatrix(0, 1) = LoadResString(1601 + RS) & "   " '"Immissionsberechnung für: "
    MSFlex0.TextMatrix(0, 2) = frmMain.txt03.Text & ", " & frmMain.txt04.Text
    MSFlex0.TextMatrix(1, 2) = ""  'frmMain.txt05.Text   'Wohnort
    MSFlex0.TextMatrix(1, 2) = frmMain.txt05.Text   'Wohnort
    MSFlex2.TextMatrix(32, 1) = frmMain.txt05.Text & ", " & Date  'Wohnort
    MSFlex0.TextMatrix(2, 1) = LoadResString(1750 + RS) & "  " & frmMain.txt301.Text
    MSFlex0.TextMatrix(2, 2) = LoadResString(1756 + RS) & "  " & frmMain.txtA19.Text  'frmMain.lbl124.Caption   'Antenne

    MSFlex0.TextMatrix(3, 1) = LoadResString(1751 + RS) & "  " & frmMain.txt302.Text

    
    MSFlex0.TextMatrix(3, 2) = LoadResString(1752 + RS) & "  " & frmMain.txtA8.Text
    MSFlex0.TextMatrix(4, 2) = LoadResString(1753 + RS) & "  " & frmMain.txtA9.Text
    
    MSFlex0.TextMatrix(3, 4) = frmMain.txtA10.Text
    MSFlex0.TextMatrix(4, 4) = frmMain.txtA11.Text
    
    MSFlex0.TextMatrix(3, 3) = LoadResString(1754 + RS)
    MSFlex0.TextMatrix(4, 3) = LoadResString(1754 + RS)

    MSFlex0.TextMatrix(3, 5) = LoadResString(1755 + RS)
    MSFlex0.TextMatrix(4, 5) = LoadResString(1755 + RS)
  
    'Spalte 1 links ausrichten
    Flex1.ColAlignment(1) = 1
    
    
    'Texte MSFlex2
   
'    MSFlex2.TextMatrix(0, 2) = LoadResString(1759 + RS) & " " & LoadResString(1760 + RS)
    
'    MSFlex2.TextMatrix(2, 1) = LoadResString(1209 + RS)  'fett
'    MSFlex2.TextMatrix(2, 2) = LoadResString(1721 + RS)
'    MSFlex2.TextMatrix(3, 1) = LoadResString(1213 + RS)
'    MSFlex2.TextMatrix(3, 2) = LoadResString(1722 + RS)
'    MSFlex2.TextMatrix(4, 2) = LoadResString(1723 + RS)
'    MSFlex2.TextMatrix(5, 2) = LoadResString(1724 + RS)
'    MSFlex2.TextMatrix(6, 1) = LoadResString(1221 + RS)
'    MSFlex2.TextMatrix(6, 2) = LoadResString(1726 + RS)
'    MSFlex2.TextMatrix(7, 1) = LoadResString(1223 + RS)
'    MSFlex2.TextMatrix(7, 2) = LoadResString(1727 + RS)
'    MSFlex2.TextMatrix(8, 1) = LoadResString(1225 + RS)
 '   MSFlex2.TextMatrix(8, 2) = LoadResString(1728 + RS)
'   MSFlex2.TextMatrix(9, 1) = LoadResString(1228 + RS)
'    MSFlex2.TextMatrix(9, 2) = " "
'    MSFlex2.TextMatrix(10, 2) = " "
'    MSFlex2.TextMatrix(11, 2) = " "
    
'    MSFlex2.TextMatrix(12, 1) = LoadResString(1231 + RS)
'    MSFlex2.TextMatrix(14, 1) = LoadResString(1234 + RS)
'    MSFlex2.TextMatrix(14, 2) = LoadResString(1729 + RS)
 '   MSFlex2.TextMatrix(15, 1) = LoadResString(1237 + RS)
'    MSFlex2.TextMatrix(15, 2) = LoadResString(1730 + RS)
'    MSFlex2.TextMatrix(16, 1) = LoadResString(1239 + RS)
'    MSFlex2.TextMatrix(16, 2) = LoadResString(1731 + RS)
'    MSFlex2.TextMatrix(17, 1) = LoadResString(1242 + RS)
'    MSFlex2.TextMatrix(17, 2) = LoadResString(1732 + RS)
 '   MSFlex2.TextMatrix(18, 1) = LoadResString(1245 + RS)
'    MSFlex2.TextMatrix(18, 2) = LoadResString(1733 + RS)
'    MSFlex2.TextMatrix(19, 1) = LoadResString(1248 + RS)
'    MSFlex2.TextMatrix(19, 2) = LoadResString(1734 + RS)
'    MSFlex2.TextMatrix(20, 1) = LoadResString(1250 + RS)
'    MSFlex2.TextMatrix(20, 2) = LoadResString(1735 + RS)
'    MSFlex2.TextMatrix(21, 1) = LoadResString(1253 + RS)
'    MSFlex2.TextMatrix(21, 2) = LoadResString(1736 + RS)
'    MSFlex2.TextMatrix(22, 1) = LoadResString(1256 + RS)
'    MSFlex2.TextMatrix(22, 2) = LoadResString(1737 + RS)
'    MSFlex2.TextMatrix(23, 1) = LoadResString(1259 + RS)
'    MSFlex2.TextMatrix(23, 2) = LoadResString(1738 + RS)
'    MSFlex2.TextMatrix(24, 1) = LoadResString(1261 + RS)
'    MSFlex2.TextMatrix(24, 2) = LoadResString(1739 + RS)
'    MSFlex2.TextMatrix(25, 1) = LoadResString(1264 + RS)
'    MSFlex2.TextMatrix(25, 2) = LoadResString(1740 + RS)
'    MSFlex2.TextMatrix(26, 1) = LoadResString(1266 + RS)
'    MSFlex2.TextMatrix(26, 2) = LoadResString(1741 + RS)
'    MSFlex2.TextMatrix(27, 2) = LoadResString(1742 + RS)
'    MSFlex2.TextMatrix(28, 1) = LoadResString(1270 + RS)
'    MSFlex2.TextMatrix(28, 2) = LoadResString(1743 + RS)
'    MSFlex2.TextMatrix(29, 1) = LoadResString(1273 + RS)
'    MSFlex2.TextMatrix(29, 2) = LoadResString(1744 + RS)
'    MSFlex2.TextMatrix(30, 2) = LoadResString(1761 + RS)
'    MSFlex2.TextMatrix(32, 1) = frmMain.txt05.Text & ", " & Date  'Ort und Datum
    
'    MSFlex2.TextMatrix(32, 2) = LoadResString(1745 + RS) 'Unterschrift
'=
    
    If frmMain.FlexKabel2.TextMatrix(6, 4) > 0 Then     'Wert Kabellänge nur Eintragen wenn > 0m
'    MSFlex2.TextMatrix(9, 2) = frmMain.FlexKabel2.TextMatrix(6, 4) & " m  " & frmMain.cboK31.List(frmMain.cboK31.ListIndex)
    MSFlex2.TextMatrix(11, 2) = frmMain.FlexKabel2.TextMatrix(6, 4) & " m  " & frmMain.cboK31.List(frmMain.cboK31.ListIndex)
    
    Else
    MSFlex2.TextMatrix(11, 2) = ""
    End If
    
    If frmMain.FlexKabel2.TextMatrix(11, 4) > 0 Then    'Wert Kabellänge nur Eintragen wenn > 0m
'    MSFlex2.TextMatrix(10, 2) = frmMain.FlexKabel2.TextMatrix(11, 4) & " m  " & frmMain.cboK131.List(frmMain.cboK131.ListIndex)
    MSFlex2.TextMatrix(12, 2) = frmMain.FlexKabel2.TextMatrix(11, 4) & " m  " & frmMain.cboK131.List(frmMain.cboK131.ListIndex)
    Else
    MSFlex2.TextMatrix(12, 2) = ""
    End If

    If frmMain.FlexKabel2.TextMatrix(16, 4) > 0 Then    'Wert Kabellänge nur Eintragen wenn > 0m
'    MSFlex2.TextMatrix(11, 2) = frmMain.FlexKabel2.TextMatrix(16, 4) & " m  " & frmMain.cboK231.List(frmMain.cboK231.ListIndex)
    MSFlex2.TextMatrix(13, 2) = frmMain.FlexKabel2.TextMatrix(16, 4) & " m  " & frmMain.cboK231.List(frmMain.cboK231.ListIndex)
    Else
    MSFlex2.TextMatrix(13, 2) = ""
    End If


'    MSFlex2.TextMatrix(4, 2) = LoadResString(1723 + RS) & "  " & frmMain.txt25.Text     'Horizontalprojektion
'    MSFlex2.TextMatrix(5, 2) = LoadResString(1724 + RS) & "  " & frmMain.txt35.Text     'Effektive Distanz
    MSFlex2.TextMatrix(5, 2) = LoadResString(1723 + RS) & "  " & frmMain.txt25.Text     'Horizontalprojektion
    MSFlex2.TextMatrix(6, 2) = LoadResString(1724 + RS) & "  " & frmMain.txt35.Text     'Effektive Distanz

    If frmMain.FlexKabel2.TextMatrix(23, 4) > 0 Then
'    MSFlex2.TextMatrix(12, 2) = frmMain.FlexKabel2.TextMatrix(23, 1) & "  " & frmMain.FlexKabel2.TextMatrix(23, 4) & " dB"   'übrige Dämpfung 1.Linie
    MSFlex2.TextMatrix(14, 2) = frmMain.FlexKabel2.TextMatrix(23, 1) & "  " & frmMain.FlexKabel2.TextMatrix(23, 4) & " dB"   'übrige Dämpfung 1.Linie
    Else
    MSFlex2.TextMatrix(14, 2) = ""
    End If
    
    If frmMain.FlexKabel2.TextMatrix(24, 4) > 0 Then
'    MSFlex2.TextMatrix(13, 2) = frmMain.FlexKabel2.TextMatrix(24, 1) & "  " & frmMain.FlexKabel2.TextMatrix(24, 4) & " dB"   'übrige Dämpfung 2.Linie
    MSFlex2.TextMatrix(15, 2) = frmMain.FlexKabel2.TextMatrix(24, 1) & "  " & frmMain.FlexKabel2.TextMatrix(24, 4) & " dB"   'übrige Dämpfung 2.Linie
    Else
    MSFlex2.TextMatrix(15, 2) = ""
    End If
    
    Fettschrift     'Sub
      
      
    'Anzeige von Flex1 bei reg Kopien ===================
    
 
    
        MSFlex0.TextMatrix(4, 1) = Label1.Caption
    

End Sub

'==========


Private Sub lblGrid3_Change()

'File Auf Harddisk speichern

'On Error Resume Next
On Error GoTo ErrHandler

    Flex1.TextMatrix(0, 0) = frmMain.lblK2.Caption  'Spez für Kabeldaten-Liste erkennen
    
    
    RichTextBox1.Text = ""
    RichTextBox1.Text = MSFlex0.Rows
        
    MSFlex0.Redraw = False
    
    Dim Y As Long
    For Y = 0 To MSFlex0.Rows - 1
        Dim X As Long
        For X = 0 To MSFlex0.Cols - 1
            MSFlex0.Col = X
            MSFlex0.Row = Y
            RichTextBox1.Text = RichTextBox1.Text + "|" + MSFlex0.Text
        Next X
    Next Y
    
    RichTextBox1.Text = RichTextBox1.Text + "|" + vbCrLf
    
    MSFlex0.Redraw = True
        
   
    Flex1.Redraw = False
    
    For Y = 0 To Flex1.Rows - 1
        For X = 0 To Flex1.Cols - 1
            Flex1.Col = X
            Flex1.Row = Y
            RichTextBox1.Text = RichTextBox1.Text + ";" + Flex1.Text
        Next X
    Next Y
    
    RichTextBox1.Text = RichTextBox1.Text + ";" + vbCrLf
    
    Flex1.Redraw = True

'=======MSFlex 2 neu

   
    MSFlex2.Redraw = False
    
    For Y = 0 To MSFlex2.Rows - 1
        For X = 0 To MSFlex2.Cols - 1
            MSFlex2.Col = X
            MSFlex2.Row = Y
            RichTextBox1.Text = RichTextBox1.Text + "#" + MSFlex2.Text
        Next X
    Next Y
    
    RichTextBox1.Text = RichTextBox1.Text + "#"
    
    MSFlex2.Redraw = True


'======Ende MSFlex 2 neu

    CommonDialog1.CancelError = True
    On Error GoTo ErrHandler
    
    CommonDialog1.InitDir = App.Path & "\Berechnungen"        'Dir einstellen"
    
    CommonDialog1.Filter = "Flat File (*.ber)|*.ber|All Files (*.*)|*.*"
    CommonDialog1.Flags = cdlOFNCreatePrompt + cdlOFNOverwritePrompt + cdlOFNPathMustExist + cdlOFNHideReadOnly
    CommonDialog1.FilterIndex = 1        ' bei Dateityp, wird *.shw angeboten
    CommonDialog1.ShowSave
    RichTextBox1.SaveFile (CommonDialog1.FileName)
    
    
ErrHandler:
   If Not Err = cdlCancel Then Resume Next
       
End Sub

'===========


Private Sub lblGrid4_Change()

    'Berechnung vom Disk laden und in RichTextBox schreiben



On Error GoTo Dateifehler_Err
        
    CommonDialog1.CancelError = True                'CancelError auf True setzen
    CommonDialog1.InitDir = App.Path & "\berechnungen"        'Dir einstellen"
    CommonDialog1.Flags = cdlOFNHideReadOnly        'Attribute setzen
    CommonDialog1.Filter = "Flat File (*.ber)|*.ber|All Files (*.*)|*.*"
    CommonDialog1.FilterIndex = 1                   'bei Dateityp, wird *.shw angeboten
    CommonDialog1.ShowOpen                          'Dialogfeld "Öffnen" anzeigen
    RichTextBox1.LoadFile (CommonDialog1.FileName)  'Datei wird in die RichTextBox geschriebe
    
 '=========== neu MSFlex 0 ==================


Dateifehler_Err:
    
    Label2.Caption = Err.Number
    If Label2.Caption > 0 Then
        Label2.BackColor = vbYellow
    End If
    
    Select Case Err.Number
    
    'Fehlermeldung
    Case 20
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
    
    Case 53
    MsgBox Error, 16, "Datei nicht gefunden"
    Exit Sub
           
    Case 75
    MsgBox Error, 16, "Pfadeingabe falsch ausgefüllt"
    Exit Sub
    
    Case 76
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
     
    Case 13
    MsgBox Error, 16, "Datei nicht für diese Version des Programms "
    Exit Sub
       
    Case 327
    MsgBox Error, 16, "Pfadeingabe  "
    Exit Sub

    
    Case 16
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
   
    
    End Select
 
'========
'Wenn eine Fehlermeldung vorliegt Ausstieg aus Programm
    If Label2.Caption > 0 Then
        Label2.Caption = "0"
    Exit Sub
    Else

        'Auslesen der Richtextbox und Flex füllen
        Rich_auslesen 'Sub ausführen
    End If

End Sub



Private Sub Rich_auslesen()

'Auslesen der Richtextbox und Flex füllen

On Error Resume Next
       
    MSFlex0.Clear
             
    Dim längezeilen1 As Long
    längezeilen1 = InStr(RichTextBox1.Text, "|")
    RichTextBox1.SelLength = längezeilen1 - 1
    MSFlex0.Rows = RichTextBox1.SelText
    
    MSFlex0.Redraw = False
    
    Dim x1 As Long                                                        ' Variable für die Spalten
    Dim y1 As Long                                                        ' Variable für die Zeilen
    Dim Start As Long                                                    'Variable für den Markierungsanfang
    Dim Ende As Long                                                     'Variable für das Markierungsende
    Ende = 1
    For y1 = 0 To MSFlex0.Rows - 1                              'ermittelt die Anzahlt der Zeilen
        For x1 = 0 To MSFlex0.Cols - 1                                'ermittelt die Anzahlt der Spalten
            Start = InStr(Ende, RichTextBox1.Text, "|", 0)          ' Sucht nach dem Simikolon und gibt die Position in einer Zahl aus,
            RichTextBox1.SelStart = Start                                ' Markierungsanfang setzen
            Ende = InStr(Start + 1, RichTextBox1.Text, "|", 0)  'Markierungsende errechnen
            RichTextBox1.SelLength = Ende - 1 - Start                    ' Markierungsende setzen.
                    
            MSFlex0.Col = x1
            MSFlex0.Row = y1

            MSFlex0.Text = RichTextBox1.SelText
            
            MSFlex0.Col = x1
        Next x1
    Next y1
   
    'Aktiviert das Neuzeichnen des FlexTabelle.
    MSFlex0.Redraw = True

 
 '========Ende MSFlex 0 neu ======================
    
    
    Flex1.Clear
                        
    fGrid1.Caption = (CommonDialog1.FileName)
            
           
    Dim längezeilen As Long
    längezeilen = InStr(RichTextBox1.Text, ";")
    RichTextBox1.SelLength = längezeilen - 1
   ' Flex1.Rows = RichTextBox1.SelText
    
    Flex1.Redraw = False
    
    Dim X As Long                                                        ' Variable für die Spalten
    Dim Y As Long                                                        ' Variable für die Zeilen
    Ende = 1
    For Y = 0 To Flex1.Rows - 1                                          'ermittelt die Anzahlt der Zeilen
        For X = 0 To Flex1.Cols - 1                                      'ermittelt die Anzahlt der Spalten
            Start = InStr(Ende, RichTextBox1.Text, ";", 0)               ' Sucht nach dem Simikolon und gibt die Position in einer Zahl aus,
            RichTextBox1.SelStart = Start                                ' Markierungsanfang setzen
            Ende = InStr(Start + 1, RichTextBox1.Text, ";", 0)           'Markierungsende errechnen
            RichTextBox1.SelLength = Ende - 1 - Start                    ' Markierungsende setzen.
                    
            Flex1.Col = X
            Flex1.Row = Y

            Flex1.Text = RichTextBox1.SelText
            
            Flex1.Col = X
        Next X
    Next Y
    
    'Aktiviert das Neuzeichnen des FlexTabelle.
    Flex1.Redraw = True

'====== neu MSFlex2 ===========
    MSFlex2.Clear
       
    Dim längezeilen2 As Long
    längezeilen2 = InStr(RichTextBox1.Text, "#")
    RichTextBox1.SelLength = längezeilen2 - 1
 '   MSFlex2.Rows = RichTextBox1.SelText
    
    MSFlex2.Redraw = False
    
    Dim X2 As Long                                                        ' Variable für die Spalten
    Dim y2 As Long                                                        ' Variable für die Zeilen
    Ende = 1
    For y2 = 0 To MSFlex2.Rows - 1                              'ermittelt die Anzahlt der Zeilen
        For X2 = 0 To MSFlex2.Cols - 1                                'ermittelt die Anzahlt der Spalten
            Start = InStr(Ende, RichTextBox1.Text, "#", 0)          ' Sucht nach dem Simikolon und gibt die Position in einer Zahl aus,
            RichTextBox1.SelStart = Start                                ' Markierungsanfang setzen
            Ende = InStr(Start + 1, RichTextBox1.Text, "#", 0)  'Markierungsende errechnen
            RichTextBox1.SelLength = Ende - 1 - Start                    ' Markierungsende setzen.
                    
            MSFlex2.Col = X2
            MSFlex2.Row = y2

            MSFlex2.Text = RichTextBox1.SelText
            
            MSFlex2.Col = X2
        Next X2
    Next y2

'ErrHandler:
'   If Not Err = cdlCancel Then Resume Next
'====
            
            
    
'Aktiviert das Neuzeichnen des FlexTabelle.
    MSFlex2.Redraw = True

'======Ende MSFlex 2 =========


    'Kabeltypenliste aus gespeicherten Berechnung auslesen und laden
    
    If fGrid1.Flex1.TextMatrix(0, 0) = "" Then
    
    Else
        frmMain.lblK2.Caption = fGrid1.Flex1.TextMatrix(0, 0) 'Auslesen der aktuellen Kabeltypenliste
        frmMain.DoKabeldaten_load        'Kabeldaten laden
        frmMain.DoKabeldatenuebertragen  'Kabeldaten von Flexgrid in Listbox übertragen
    End If
'=====
    
Fettschrift     'Sub
   
zurückkopieren ' Sub


    MSFlex0.TextMatrix(4, 1) = Label1.Caption

   
End Sub

'-----------------------------------------------




'==============neu Zurückkopieren auf Berechnungsformular==================

Private Sub zurückkopieren1_click()

    zurückkopieren 'Sub

End Sub


Private Sub zurückkopieren()
      
    Dim Min As Integer
    'Fortschrittsanzeige
    ProgressBar1.Value = Min
    ProgressBar1.Visible = True
  
      
    cboA1_sperren = 1  'Sperrt Antennen bearbeitung
   
 On Error Resume Next
 
    Dim Inhalt As String
    Dim TotalInhalt As String
    Dim Länge As Integer
    Dim KTyp As String
    Dim Totallänge As Integer
    Dim Pos As Integer
    Dim Pos1 As Integer
    Dim Pos2 As Integer
    Dim Pos3 As Integer
    Dim Asci As String
    'Hersteller laden
    Herst = MSFlex0.TextMatrix(5, 1)
    If Herst = Asci Then
    Herst = "24_Diverse"
    End If
    frmMain.Herst_typen
    
    
    'Antennentypen laden
    Ty = MSFlex0.TextMatrix(5, 2)
    If Ty = Asci Then
    Ty = "Dipol"
    End If
    frmMain.SelectAntTyp
    
    'Fortschrittsanzeige erhöhen um 1
    ProgressBar1.Value = ProgressBar1.Value + 1
    
'Eintragen der berechneten Werte in FlexGrid.
'Name und Adresse einfüllen


'    MSFlex0.TextMatrix(0, 2) = frmMain.txt03.Text & ", " & frmMain.txt04.Text
'   Vorname Name
    Inhalt = MSFlex0.TextMatrix(0, 2)
    Pos = InStr(1, Inhalt, ",")
    frmMain.txt03.Text = Mid(MSFlex0.TextMatrix(0, 2), 1, Pos - 1)
    frmMain.txt04.Text = Mid(MSFlex0.TextMatrix(0, 2), Pos + 2)
   
    
'    MSFlex0.TextMatrix(1, 2) = frmMain.txt05.Text
'   PLZ, Ort
    frmMain.txt05.Text = MSFlex0.TextMatrix(1, 2)
    
'    MSFlex0.TextMatrix(2, 1) = LoadResString(1750 + RS) & "  " & frmMain.txt301.Text
'   Sender
    Inhalt = LoadResString(1750 + RS) & "  "
    Länge = Len(Inhalt)
    TotalInhalt = MSFlex0.TextMatrix(2, 1)
    Totallänge = Len(TotalInhalt)
    frmMain.txt301.Text = MSFlex0.TextMatrix(2, 1)
    frmMain.txt301.Text = Mid(frmMain.txt301.Text, Länge, Totallänge + 1 - Länge)
    frmMain.txt301.Text = Mid(frmMain.txt301.Text, 2)

'    MSFlex0.TextMatrix(2, 2) = LoadResString(1756 + RS) & "  " & frmMain.txtA19.Text  'frmMain.lbl124.Caption   'Antenne
'   Antennentyp
    Inhalt = LoadResString(1756 + RS) & "  "
    Länge = Len(Inhalt)
    TotalInhalt = MSFlex0.TextMatrix(2, 2)
    Totallänge = Len(TotalInhalt)
    frmMain.txtA19.Text = MSFlex0.TextMatrix(2, 2)
    frmMain.txtA19.Text = Mid(frmMain.txtA19.Text, Länge, Totallänge + 1 - Länge)

'    MSFlex0.TextMatrix(3, 1) = LoadResString(1751 + RS) & "  " & frmMain.txt302.Text
'   Endstufe
    Inhalt = LoadResString(1751 + RS) & "  "
    Länge = Len(Inhalt)
    TotalInhalt = MSFlex0.TextMatrix(3, 1)
    Totallänge = Len(TotalInhalt)
    frmMain.txt302.Text = MSFlex0.TextMatrix(3, 1)
    frmMain.txt302.Text = Mid(frmMain.txt302.Text, Länge, Totallänge + 1 - Länge)
    frmMain.txt302.Text = Mid(frmMain.txt302.Text, 2)

    
'    MSFlex0.TextMatrix(3, 2) = LoadResString(1752 + RS) & "  " & frmMain.txtA8.Text
'   Horizontal drehbar ja,nein
    Inhalt = LoadResString(1752 + RS) & "  "
    Länge = Len(Inhalt)
    TotalInhalt = MSFlex0.TextMatrix(3, 2)
    Totallänge = Len(TotalInhalt)
    frmMain.txtA8.Text = MSFlex0.TextMatrix(3, 2)
    frmMain.txtA8.Text = Mid(frmMain.txtA8.Text, Länge, Totallänge + 1 - Länge)
    frmMain.txtA8.Text = Mid(frmMain.txtA8.Text, 2, 6)

'    MSFlex0.TextMatrix(4, 2) = LoadResString(1753 + RS) & "  " & frmMain.txtA9.Text
'   Vertikal drehbar ja,nein
    Inhalt = LoadResString(1753 + RS) & "  "
    Länge = Len(Inhalt)
    TotalInhalt = MSFlex0.TextMatrix(4, 2)
    Totallänge = Len(TotalInhalt)
    frmMain.txtA9.Text = MSFlex0.TextMatrix(4, 2)
    frmMain.txtA9.Text = Mid(frmMain.txtA9.Text, Länge, Totallänge + 1 - Länge)
    frmMain.txtA9.Text = Mid(frmMain.txtA9.Text, 2, 6)
    
'    MSFlex0.TextMatrix(3, 4) = frmMain.txtA10.Text
'    Horizontal Grad
    frmMain.txtA10.Text = MSFlex0.TextMatrix(3, 4)
    
'    MSFlex0.TextMatrix(4, 4) = frmMain.txtA11.Text
'   Vertikal Grad
    frmMain.txtA11.Text = MSFlex0.TextMatrix(4, 4)
    
  
    'Spalte 1 links ausrichten
    Flex1.ColAlignment(1) = 1
    
    'Fortschrittsanzeige erhöhen um 1
    ProgressBar1.Value = ProgressBar1.Value + 1
    
     
    'Koaxkabel 1 ausfiltern
'    TotalInhalt = MSFlex2.TextMatrix(9, 2)
'    Totallänge = Len(TotalInhalt)
'    Pos = InStr(1, TotalInhalt, "m")
    'Kabellänge ausfiltern
'    frmMain.FlexKabel2.TextMatrix(6, 4) = Mid(MSFlex2.TextMatrix(9, 2), 1, Pos - 1)
'    Pos1 = InStr(Pos, TotalInhalt, (" "))
'    Pos2 = InStr(Pos1 + 3, TotalInhalt, " ")
'    Pos3 = InStr(Pos2 + 3, TotalInhalt, " ")
    'Kabeltyp ausfiltern
''    KTyp = Mid(MSFlex2.TextMatrix(9, 2), Pos1 + 2, Pos2 - (Pos1 + 2))
'    KTyp = Mid(MSFlex2.TextMatrix(9, 2), Pos1 + 2, Totallänge - Pos2)

    'Koaxkabel 1 ausfiltern
    TotalInhalt = MSFlex2.TextMatrix(11, 2)
    Totallänge = Len(TotalInhalt)
    Pos = InStr(1, TotalInhalt, "m")
    'Kabellänge ausfiltern
    frmMain.FlexKabel2.TextMatrix(6, 4) = Mid(MSFlex2.TextMatrix(11, 2), 1, Pos - 1)
    Pos1 = InStr(Pos, TotalInhalt, (" "))
    Pos2 = InStr(Pos1 + 3, TotalInhalt, " ")
    Pos3 = InStr(Pos2 + 3, TotalInhalt, " ")
    'Kabeltyp ausfiltern
'    KTyp = Mid(MSFlex2.TextMatrix(9, 2), Pos1 + 2, Pos2 - (Pos1 + 2))
    KTyp = Mid(MSFlex2.TextMatrix(11, 2), Pos1 + 2, Totallänge - Pos2)

    'Dämpfung ausfiltern
'    frmMain.lblK71.Caption = Mid(MSFlex2.TextMatrix(9, 2), Pos3, -2)
    
    'Kabeltyp in ComboBox auswählen
    Dim Feld As Integer
    Dim InhaltCbo As String

    For Feld = 0 To frmMain.FlexKabel1.Cols - 2    '11
        InhaltCbo = frmMain.cboK31.List(Feld)
        If InhaltCbo = KTyp Then
            frmMain.cboK31.ListIndex = Feld
        Else
    
        End If
        
       'Fortschrittsanzeige erhöhen um 1
       ProgressBar1.Value = ProgressBar1.Value + 1

        
    Next Feld

     
    'Koaxkabel 2 ausfiltern
'    TotalInhalt = MSFlex2.TextMatrix(10, 2)
'    Totallänge = Len(TotalInhalt)
'    Pos = InStr(1, TotalInhalt, "m")
    'Kabellänge ausfiltern
'    frmMain.FlexKabel2.TextMatrix(11, 4) = Mid(MSFlex2.TextMatrix(10, 2), 1, Pos - 1)
'    Pos1 = InStr(Pos, TotalInhalt, (" "))
'    Pos2 = InStr(Pos1 + 3, TotalInhalt, " ")
'    Pos3 = InStr(Pos2 + 3, TotalInhalt, " ")
    'Kabeltyp ausfiltern
'    KTyp = Mid(MSFlex2.TextMatrix(10, 2), Pos1 + 2, Totallänge - Pos2)
    'Dämpfung ausfiltern
''    frmMain.lblK171.Caption = Mid(MSFlex2.TextMatrix(10, 2), Pos3, -2)
    
    'Koaxkabel 2 ausfiltern
    TotalInhalt = MSFlex2.TextMatrix(12, 2)
    Totallänge = Len(TotalInhalt)
    Pos = InStr(1, TotalInhalt, "m")
    'Kabellänge ausfiltern
    frmMain.FlexKabel2.TextMatrix(11, 4) = Mid(MSFlex2.TextMatrix(12, 2), 1, Pos - 1)
    Pos1 = InStr(Pos, TotalInhalt, (" "))
    Pos2 = InStr(Pos1 + 3, TotalInhalt, " ")
    Pos3 = InStr(Pos2 + 3, TotalInhalt, " ")
    'Kabeltyp ausfiltern
    KTyp = Mid(MSFlex2.TextMatrix(12, 2), Pos1 + 2, Totallänge - Pos2)
    
    
    'Kabeltyp in ComboBox auswählen
    For Feld = 0 To frmMain.FlexKabel1.Cols - 2
        InhaltCbo = frmMain.cboK131.List(Feld)
        If InhaltCbo = KTyp Then
            frmMain.cboK131.ListIndex = Feld
        Else
    
        End If
    
        'Fortschrittsanzeige erhöhen um 1
        ProgressBar1.Value = ProgressBar1.Value + 1
   
   
   Next Feld

'==
    'Koaxkabel 3 ausfiltern
'    TotalInhalt = MSFlex2.TextMatrix(11, 2)
'    Totallänge = Len(TotalInhalt)
'    Pos = InStr(1, TotalInhalt, "m")
    'Kabellänge ausfiltern
'    frmMain.FlexKabel2.TextMatrix(16, 4) = Mid(MSFlex2.TextMatrix(11, 2), 1, Pos - 1)
'    Pos1 = InStr(Pos, TotalInhalt, (" "))
'    Pos2 = InStr(Pos1 + 3, TotalInhalt, " ")
'    Pos3 = InStr(Pos2 + 3, TotalInhalt, " ")
    'Kabeltyp ausfiltern
'    KTyp = Mid(MSFlex2.TextMatrix(11, 2), Pos1 + 2, Totallänge - Pos2)
    'Dämpfung ausfiltern
' '   frmMain.lblK271.Caption = Mid(MSFlex2.TextMatrix(11, 2), Pos3, -2)
    
     'Koaxkabel 3 ausfiltern
    TotalInhalt = MSFlex2.TextMatrix(13, 2)
    Totallänge = Len(TotalInhalt)
    Pos = InStr(1, TotalInhalt, "m")
    'Kabellänge ausfiltern
    frmMain.FlexKabel2.TextMatrix(16, 4) = Mid(MSFlex2.TextMatrix(13, 2), 1, Pos - 1)
    Pos1 = InStr(Pos, TotalInhalt, (" "))
    Pos2 = InStr(Pos1 + 3, TotalInhalt, " ")
    Pos3 = InStr(Pos2 + 3, TotalInhalt, " ")
    'Kabeltyp ausfiltern
    KTyp = Mid(MSFlex2.TextMatrix(13, 2), Pos1 + 2, Totallänge - Pos2)
   
    
    'Kabeltyp in ComboBox auswählen
    For Feld = 0 To frmMain.FlexKabel1.Cols - 2
        InhaltCbo = frmMain.cboK231.List(Feld)
        If InhaltCbo = KTyp Then
            frmMain.cboK231.ListIndex = Feld
        Else
    
        End If
    
        'Fortschrittsanzeige erhöhen um 1
        ProgressBar1.Value = ProgressBar1.Value + 1
   
   
   Next Feld

'==
  'Cursor auf Typ setzen
    Dim Feld1 As String
    
    For Feld = 1 To frmMain.flexT1.Rows - 1    '11
        Feld1 = frmMain.flexA1.TextMatrix(1, 2)
        If Feld1 = frmMain.flexT1.TextMatrix(Feld, 2) Then
            frmMain.flexT1.Row = Feld
        Else
    
        End If
        
        
        Next Feld
        
        frmMain.flexT1.Col = 2

 
 
 'Cursor auf Hersteller setzen
 '   Dim Feld1 As String
    
    For Feld = 1 To frmMain.flexH1.Rows - 1    '11
        Feld1 = frmMain.flexT1.TextMatrix(0, 2)
        If Feld1 = frmMain.flexH1.TextMatrix(Feld, 2) Then
            frmMain.flexH1.Row = Feld
        Else
    
        End If
        
        
        Next Feld
        
        frmMain.flexH1.Col = 2

'=====
    
    'Horizontalprojektion
    Inhalt = LoadResString(1723 + RS) & "  "
    Länge = Len(Inhalt)
    TotalInhalt = MSFlex2.TextMatrix(5, 2)
    Totallänge = Len(TotalInhalt)
    frmMain.txt25.Text = MSFlex2.TextMatrix(5, 2)
    frmMain.txt25.Text = Mid(frmMain.txt25.Text, Länge, Totallänge + 1 - Länge)
    frmMain.txt25.Text = Mid(frmMain.txt25.Text, 2, 6)

    'Effektive Distanz
    Inhalt = LoadResString(1724 + RS) & "  "
    Länge = Len(Inhalt)
    TotalInhalt = MSFlex2.TextMatrix(6, 2)
    Totallänge = Len(TotalInhalt)
    frmMain.txt35.Text = MSFlex2.TextMatrix(6, 2)
    frmMain.txt35.Text = Mid(frmMain.txt35.Text, Länge, Totallänge + 1 - Länge)
    frmMain.txt35.Text = Mid(frmMain.txt35.Text, 2, 6)
    
    
    'übrige Dämpfung 1.Linie
'    TotalInhalt = MSFlex2.TextMatrix(12, 2)
'    Totallänge = Len(TotalInhalt)
'    Pos = InStr(1, TotalInhalt, " ")
    'Text und Dämpfung ausfiltern
'    Pos1 = InStr(Pos, TotalInhalt, ("."))
'    frmMain.FlexKabel2.TextMatrix(23, 1) = Mid(MSFlex2.TextMatrix(12, 2), 1, Pos1 - 3)
'    frmMain.FlexKabel2.TextMatrix(23, 4) = Mid(MSFlex2.TextMatrix(12, 2), Pos1 - 2, 5)

    'übrige Dämpfung 2.Linie
'    TotalInhalt = MSFlex2.TextMatrix(13, 2)
'    Totallänge = Len(TotalInhalt)
'    Pos = InStr(1, TotalInhalt, " ")
    'Text und Dämpfung ausfiltern
'    Pos1 = InStr(Pos, TotalInhalt, ("."))
'    frmMain.FlexKabel2.TextMatrix(24, 1) = Mid(MSFlex2.TextMatrix(13, 2), 1, Pos1 - 3)
'    frmMain.FlexKabel2.TextMatrix(24, 4) = Mid(MSFlex2.TextMatrix(13, 2), Pos1 - 2, 5)

    'übrige Dämpfung 1.Linie
    TotalInhalt = MSFlex2.TextMatrix(14, 2)
    Totallänge = Len(TotalInhalt)
    Pos = InStr(1, TotalInhalt, " ")
    'Text und Dämpfung ausfiltern
    Pos1 = InStr(Pos, TotalInhalt, ("."))
    frmMain.FlexKabel2.TextMatrix(23, 1) = Mid(MSFlex2.TextMatrix(14, 2), 1, Pos1 - 3)
    frmMain.FlexKabel2.TextMatrix(23, 4) = Mid(MSFlex2.TextMatrix(14, 2), Pos1 - 2, 5)

    'übrige Dämpfung 2.Linie
    TotalInhalt = MSFlex2.TextMatrix(15, 2)
    Totallänge = Len(TotalInhalt)
    Pos = InStr(1, TotalInhalt, " ")
    'Text und Dämpfung ausfiltern
    Pos1 = InStr(Pos, TotalInhalt, ("."))
    frmMain.FlexKabel2.TextMatrix(24, 1) = Mid(MSFlex2.TextMatrix(15, 2), 1, Pos1 - 3)
    frmMain.FlexKabel2.TextMatrix(24, 4) = Mid(MSFlex2.TextMatrix(15, 2), Pos1 - 2, 5)
    
    
        'Kopieren der berechnenten Werte in frmMain
    
    Dim Reihe As Integer
    Dim Spalte As Integer
    
    Spalte = C1
    
    For Spalte = 1 To 5
    
        For Reihe = 0 To 3
            'Ueberspringt Kolonne wenn kein Inhalt
            If Flex1.TextMatrix(Reihe, Spalte + 3) = Ascii Then
            Else
        
               frmMain.FlexGrid2.TextMatrix(Reihe, Spalte) = Flex1.TextMatrix(Reihe, Spalte + 3)
           
            End If
            
            'Fortschrittsanzeige erhöhen um 1
            If ProgressBar1.Value = 100 Then
            
            Else
                ProgressBar1.Value = ProgressBar1.Value + 1
            End If
    
        Next Reihe
        
        For Reihe = 4 To 28 '27
            'Ueberspringt Kolonne wenn kein Inhalt
            If Flex1.TextMatrix(Reihe, Spalte + 3) = Ascii Then
            Else
        
               frmMain.txtA2.Text = frmMain.FlexGrid2.TextMatrix(10, Spalte)
               frmMain.FlexGrid2.TextMatrix(Reihe + 1, Spalte) = Flex1.TextMatrix(Reihe, Spalte + 3)
           
            End If
            
            'Fortschrittsanzeige erhöhen um 1
            If ProgressBar1.Value = 100 Then
            
            Else
                ProgressBar1.Value = ProgressBar1.Value + 1
            End If
            
        Next Reihe
        
               frmMain.FlexGrid2.TextMatrix(4, Spalte) = Flex1.TextMatrix(23, Spalte + 3)
        
        
    Next Spalte

    cboA1_sperren = 1  'Sperrt Antennen bearbeitung
    
    'Frequenzwahl auf entsprechende Frequenz setzen durch Kolonnenwechsel
    frmMain.FlexGrid2.LeftCol = 2  'auf erste Kolonne setzen
    frmMain.FlexGrid2.LeftCol = 1  'auf erste Kolonne setzen
     
    Fettschrift     'Sub
       
    'Anzeige von registrierten Kopien in Flex1  ===================
    MSFlex0.TextMatrix(4, 1) = Label1.Caption
    
    cboA1_sperren = 0  'Freigabe der Antennen bearbeitung
    
    'Winkeldämpfung anzeigen
    frmMain.flexA1.Col = MSFlex0.TextMatrix(5, 3)
    
    frmMain.Winkeldämpf_anzeigen   'Sub
  
    ProgressBar1.Visible = False  'Fortschrittanzeige unterdrücken
    ProgressBar1.Value = Min      'Fortschrittanzeige auf min. setzen


End Sub

'========================Ende zurückkopieren=================================








