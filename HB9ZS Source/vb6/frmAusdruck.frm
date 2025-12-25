VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form frmAusdruck 
   Caption         =   "Druckformular"
   ClientHeight    =   12990
   ClientLeft      =   165
   ClientTop       =   555
   ClientWidth     =   12255
   LinkTopic       =   "Form1"
   ScaleHeight     =   12990
   ScaleWidth      =   12255
   StartUpPosition =   3  'Windows-Standard
   Begin VB.Frame Frame1 
      BorderStyle     =   0  'Kein
      Height          =   13935
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   13245
      Begin VB.Frame Frame2 
         Height          =   1095
         Left            =   600
         TabIndex        =   1
         Top             =   120
         Width           =   7215
         Begin MSFlexGridLib.MSFlexGrid MSFlex01 
            Height          =   975
            Left            =   120
            TabIndex        =   2
            Top             =   240
            Width           =   7215
            _ExtentX        =   12726
            _ExtentY        =   1720
            _Version        =   393216
            BackColorFixed  =   16777215
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
      Begin MSFlexGridLib.MSFlexGrid MSFlex11 
         Height          =   7215
         Left            =   600
         TabIndex        =   3
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
      Begin MSFlexGridLib.MSFlexGrid MSFlex21 
         Height          =   3375
         Left            =   240
         TabIndex        =   4
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
      Begin VB.Label Label1 
         Height          =   255
         Left            =   7680
         TabIndex        =   5
         Top             =   480
         Visible         =   0   'False
         Width           =   2415
      End
   End
End
Attribute VB_Name = "frmAusdruck"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'=============================================================
' Ausdruck Formular
'
'wird beim Betätigen von Drucken in dieses Formular kopiert
'
'
'
'10.10.2003/hb9zs
'===============================================================

Option Explicit

Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long

Private Const WM_USER = &H400
Private Const VP_FORMATRANGE = WM_USER + 125
Private Const VP_YESIDO = 456654

Private Type RECT
    Left    As Long
    Top     As Long
    Right   As Long
    Bottom  As Long
End Type

Private Type TFormatRange
    hDC         As Long
    hDCTarget   As Long
    rc          As RECT
    rcPage      As RECT
End Type

Dim Ascii As String

Public Sub Formular_kopieren()

    
    Dim Reihe As Integer
    Dim Spalte As Integer
 
    'Löschen
    For Reihe = 0 To 4
        For Spalte = 1 To 5
        
                MSFlex01.TextMatrix(Reihe, Spalte) = ""
 
        Next Spalte
        
    Next Reihe


    For Reihe = 0 To 22
        For Spalte = 1 To 8
        
                MSFlex11.TextMatrix(Reihe, Spalte) = ""
 
        Next Spalte
        
    Next Reihe
    
    For Reihe = 0 To 32
        For Spalte = 1 To 2
        
                MSFlex21.TextMatrix(Reihe, Spalte) = ""
 
        Next Spalte
        
    Next Reihe
 
 
    'Kopieren
    
    For Reihe = 0 To 4
        For Spalte = 1 To 5
        
                MSFlex01.TextMatrix(Reihe, Spalte) = fGrid1.MSFlex0.TextMatrix(Reihe, Spalte)
 
        Next Spalte
        
    Next Reihe


    For Reihe = 0 To 22
        For Spalte = 1 To 8
        
                MSFlex11.TextMatrix(Reihe, Spalte) = fGrid1.Flex1.TextMatrix(Reihe, Spalte)
 
        Next Spalte
        
    Next Reihe
    
    For Reihe = 0 To 32
        For Spalte = 1 To 2
        
                MSFlex21.TextMatrix(Reihe, Spalte) = fGrid1.MSFlex2.TextMatrix(Reihe, Spalte)
 
        Next Spalte
        
    Next Reihe




End Sub

Public Sub Formular_drucken()
    
'Formular drucken

    Dim tRange      As TFormatRange
    Dim lReturn     As Long
    Dim LeftMargin As Integer
    Dim TopMargin As Integer
    Dim RightMargin As Integer
    Dim BottomMargin As Integer

 'Druckvorgang in Statuszeile anzeigen
    frmMain1.StatusBar1.Panels(1) = "  Printer activ"
    fGrid1.StatusBar1.Panels(1) = "  Printer activ"
   
    
    'Blattformat A4 hoch
        
    Printer.Orientation = vbPRORPortrait
    

    '=== MSFlex 0 ==================================================

    lReturn = SendMessage(MSFlex01.hwnd, VP_FORMATRANGE, 1, 0)

    If lReturn = VP_YESIDO Then

        'Struktur mit Formatierungsinformationen füllen
        Printer.ScaleMode = vbPixels
        With tRange
            .hDC = Printer.hDC
            'Höhe und Breite einer Seite (in Pixel)
            .rcPage.Right = Printer.ScaleWidth
            .rcPage.Bottom = Printer.ScaleHeight
            'Lage und Abmessungen des Bereichs auf den gedruckt werden soll (in Pixel)
            .rc.Left = Printer.ScaleX(LeftMargin + 20, vbMillimeters)
            .rc.Top = Printer.ScaleY(TopMargin + 15, vbMillimeters)
            '.rc.Top = Printer.ScaleY(TopMargin + 10, vbMillimeters)
            .rc.Right = .rcPage.Right - Printer.ScaleX(RightMargin, vbMillimeters)
            .rc.Bottom = .rcPage.Bottom - Printer.ScaleY(BottomMargin, vbMillimeters)
        End With

        'Drucker initialisieren
        Printer.Print vbNullString

        'Seite(n) drucken
        Do
            lReturn = SendMessage(MSFlex01.hwnd, VP_FORMATRANGE, 0, VarPtr(tRange))
            If lReturn < 0 Then
                Exit Do
            Else
                Printer.NewPage
            End If
        Loop
        
        lReturn = SendMessage(MSFlex01.hwnd, VP_FORMATRANGE, 0, 0)

    End If


    
    '==== Flex 1 ==================================================

    lReturn = SendMessage(MSFlex11.hwnd, VP_FORMATRANGE, 1, 0)

    If lReturn = VP_YESIDO Then

        'Struktur mit Formatierungsinformationen füllen
        Printer.ScaleMode = vbPixels
        With tRange
            .hDC = Printer.hDC
            'Höhe und Breite einer Seite (in Pixel)
            .rcPage.Right = Printer.ScaleWidth
            .rcPage.Bottom = Printer.ScaleHeight
            'Lage und Abmessungen des Bereichs auf den gedruckt werden soll (in Pixel)
            .rc.Left = Printer.ScaleX(LeftMargin + 20, vbMillimeters)
            .rc.Top = Printer.ScaleY(TopMargin + 38, vbMillimeters)
            .rc.Right = .rcPage.Right - Printer.ScaleX(RightMargin, vbMillimeters)
            .rc.Bottom = .rcPage.Bottom - Printer.ScaleY(BottomMargin, vbMillimeters)
        End With

        'Drucker initialisieren
        Printer.Print vbNullString

        'Seite(n) drucken
        Do
            lReturn = SendMessage(MSFlex11.hwnd, VP_FORMATRANGE, 0, VarPtr(tRange))
            If lReturn < 0 Then
                Exit Do
            Else
                Printer.NewPage
            End If
        Loop

        lReturn = SendMessage(MSFlex11.hwnd, VP_FORMATRANGE, 0, 0)

    End If

    '=== MSFlex 2 ==================================================

    lReturn = SendMessage(MSFlex21.hwnd, VP_FORMATRANGE, 1, 0)

    If lReturn = VP_YESIDO Then

        'Struktur mit Formatierungsinformationen füllen
        Printer.ScaleMode = vbPixels
        With tRange
            .hDC = Printer.hDC
            'Höhe und Breite einer Seite (in Pixel)
            .rcPage.Right = Printer.ScaleWidth
            .rcPage.Bottom = Printer.ScaleHeight
            'Lage und Abmessungen des Bereichs auf den gedruckt werden soll (in Pixel)
            .rc.Left = Printer.ScaleX(LeftMargin + 20, vbMillimeters)
            .rc.Top = Printer.ScaleY(TopMargin + 140, vbMillimeters)
            .rc.Right = .rcPage.Right - Printer.ScaleX(RightMargin, vbMillimeters)
            .rc.Bottom = .rcPage.Bottom - Printer.ScaleY(BottomMargin, vbMillimeters)
        End With

        'Drucker initialisieren
        Printer.Print vbNullString

        'Seite(n) drucken
        Do
            lReturn = SendMessage(MSFlex21.hwnd, VP_FORMATRANGE, 0, VarPtr(tRange))
            If lReturn < 0 Then
                Exit Do
            Else
    'neue Seite (zweite Seite)
                Printer.NewPage
            End If
        Loop
        
        lReturn = SendMessage(MSFlex21.hwnd, VP_FORMATRANGE, 0, 0)

    End If
        
    Printer.EndDoc

    Printer.Orientation = vbPRORPortrait
    Printer.EndDoc

'Druckvorgang in Statuszeile löschen
    frmMain1.StatusBar1.Panels(1) = ""
    fGrid1.StatusBar1.Panels(1) = ""

End Sub

'=================================

Private Sub Form_Load()


'    Dim k As Integer, t As Integer
'    Dim Zeile As Variant, Spalte As Variant
    
    
'==== Flex 0  =========================


    'Kolonnen und Reihen festlegen
    MSFlex01.Cols = 6
    MSFlex01.Rows = 5

    'Kolonnen Breite festlegen
    MSFlex01.ColWidth(0) = 0
    MSFlex01.ColWidth(1) = 3500
    MSFlex01.ColWidth(2) = 3200
    MSFlex01.ColWidth(3) = 650
    MSFlex01.ColWidth(4) = 350
    MSFlex01.ColWidth(5) = 650
    MSFlex01.Left = 0
    'linksbündige Kolonne 2
    MSFlex01.ColAlignment(2) = 2


    'Breite und Höhe vom Flexgrid festlegen
    Frame2.Left = 0
    Frame2.Height = 0 + MSFlex01.RowHeight(1) * 5 + 150
    Frame2.Width = 0 + MSFlex01.ColWidth(0) + MSFlex01.ColWidth(1) + MSFlex01.ColWidth(2) + MSFlex01.ColWidth(3) + MSFlex01.ColWidth(4) + MSFlex01.ColWidth(5) + 50
    MSFlex01.Width = 0 + MSFlex01.ColWidth(0) + MSFlex01.ColWidth(1) + MSFlex01.ColWidth(2) + MSFlex01.ColWidth(3) + MSFlex01.ColWidth(4) + MSFlex01.ColWidth(5)
    MSFlex01.Height = 0 + MSFlex01.RowHeight(1) * MSFlex01.Rows
    

'=========================================

'Flex 1 definieren

    MSFlex11.Rows = 23
    
    MSFlex11.Cols = 9
  
    
    MSFlex11.ColWidth(0) = 10
    MSFlex11.ColWidth(1) = 3300
    MSFlex11.ColWidth(2) = 650
    MSFlex11.ColWidth(3) = 600
    MSFlex11.ColWidth(4) = 900
    MSFlex11.ColWidth(5) = 900
    MSFlex11.ColWidth(6) = 900
    MSFlex11.ColWidth(7) = 900
    MSFlex11.ColWidth(8) = 900
    MSFlex11.Left = 0
    MSFlex11.Top = Frame2.Top + Frame2.Height + 100
    MSFlex11.Width = 100 + MSFlex11.ColWidth(0) + MSFlex11.ColWidth(1) + MSFlex11.ColWidth(2) + MSFlex11.ColWidth(3) + MSFlex11.ColWidth(4) + MSFlex11.ColWidth(5) + MSFlex11.ColWidth(6) + MSFlex11.ColWidth(7) + MSFlex11.ColWidth(8)
    MSFlex11.Height = 80 + MSFlex11.RowHeight(1) * 23  '28  MSFlex11.Rows
   
     
    'Spalte 1 links ausrichten
    MSFlex11.ColAlignment(1) = 1
        
    'Spalten 3-9 rechts ausrichten
    Dim A As Integer
        For A = 3 To 8
            MSFlex11.ColAlignment(A) = 7
        Next A
    

    'Giternetz anzeigen
    MSFlex11.GridLines = 1
  


 '==== MSFlex 2 ==================================

    'Kolonnen und Reihen festlegen
    MSFlex21.Cols = 3
    MSFlex21.Rows = 33
    
    'Kolonnen Breite festlegen
    MSFlex21.ColWidth(0) = 0
    MSFlex21.ColWidth(1) = 3330
    MSFlex21.ColWidth(2) = 6500
    MSFlex21.Left = 0
    
    'linksbündige Kolonnen mit Zahlen
    MSFlex21.ColAlignment(1) = 2
    MSFlex21.ColAlignment(2) = 2

    'Breite und Höhe vom Flexgrid festlegen
    MSFlex21.Top = Frame2.Top + Frame2.Height + MSFlex11.Height + 200

    MSFlex21.Width = 0 + MSFlex21.ColWidth(0) + MSFlex21.ColWidth(1) + MSFlex21.ColWidth(2)
    MSFlex21.Height = 0 + MSFlex21.RowHeight(1) * MSFlex21.Rows

    Fettschrift     'Sub

    Frame1.Height = MSFlex01.Height + MSFlex11.Height + MSFlex21.Height + 500
    Frame1.Width = MSFlex21.Width + 300 'MSFlex11.Width + 300

    'Anzeige von MSFlex11 bei reg Kopien ===================
    
'    If Mid(frmLiz.LBL1.Caption, 3, 6) = Mid(frmx.Text2.Text, 3, 6) Then
'         Label1.Caption = ""
'    Else
'        Label1.Caption = " this program is not registered"
'    End If
    
'    MSFlex01.TextMatrix(4, 1) = Label1.Caption


End Sub
        
        
Private Sub Fettschrift()

    'Def. für Fettschrift
    Dim X As Integer
    Dim Y As Integer
    Dim Kolonne As Integer


    'MSFlex01 Fettschrift Kolonnen 1 und 2
    
    For Kolonne = 1 To 1
            'Zelle selektieren
            MSFlex01.Row = 0
            MSFlex01.Col = Kolonne
            MSFlex01.CellFontBold = True
    Next Kolonne
            MSFlex01.Row = 0
            MSFlex01.Col = 1
            MSFlex01.CellAlignment = 7
            
            MSFlex01.Row = 4
            MSFlex01.Col = 1
            MSFlex01.CellFontBold = True



    ' MSFlex11 Zeile  Fett drucken
    Dim Spalte As Integer
    For Spalte = 1 To 8
        MSFlex11.Row = 0
        MSFlex11.Col = Spalte
        MSFlex11.CellFontBold = True
    Next Spalte
    
     For Spalte = 1 To 8
        MSFlex11.Row = 1
        MSFlex11.Col = Spalte
        MSFlex11.CellFontBold = True
    Next Spalte
    
    For Spalte = 1 To 8
        MSFlex11.Row = 2
        MSFlex11.Col = Spalte
        MSFlex11.CellFontBold = True
    Next Spalte
    
    For Spalte = 1 To 8
        MSFlex11.Row = 3
        MSFlex11.Col = Spalte
        MSFlex11.CellFontBold = True
    Next Spalte
    
    For Spalte = 1 To 8
        MSFlex11.Row = 6
        MSFlex11.Col = Spalte
        MSFlex11.CellFontBold = True
    Next Spalte
    
    For Spalte = 1 To 8
        MSFlex11.Row = 16
        MSFlex11.Col = Spalte
        MSFlex11.CellFontBold = True
    Next Spalte
    
    For Spalte = 1 To 8
        MSFlex11.Row = 20 '21
        MSFlex11.Col = Spalte
        MSFlex11.CellFontBold = True
    Next Spalte
    
    For Spalte = 1 To 8
        MSFlex11.Row = 22 '23
        MSFlex11.Col = Spalte
        MSFlex11.CellFontBold = True
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
            MSFlex21.Row = Y
            MSFlex21.Col = Kolonne
            MSFlex21.CellFontBold = True
        Next Kolonne

    Next X

End Sub


