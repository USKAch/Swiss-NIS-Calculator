VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form frmAntennenausdruck 
   Caption         =   "Antennendaten-Ausdruck"
   ClientHeight    =   7410
   ClientLeft      =   45
   ClientTop       =   495
   ClientWidth     =   10785
   LinkTopic       =   "Form1"
   ScaleHeight     =   7410
   ScaleWidth      =   10785
   StartUpPosition =   3  'Windows-Standard
   Begin MSFlexGridLib.MSFlexGrid MSFlexAD4 
      Height          =   375
      Left            =   480
      TabIndex        =   4
      Top             =   4200
      Width           =   6495
      _ExtentX        =   11456
      _ExtentY        =   661
      _Version        =   393216
      ScrollBars      =   0
   End
   Begin MSFlexGridLib.MSFlexGrid MSFlexAD3 
      Height          =   375
      Left            =   480
      TabIndex        =   3
      Top             =   3600
      Width           =   6495
      _ExtentX        =   11456
      _ExtentY        =   661
      _Version        =   393216
      BackColor       =   -2147483633
      BackColorSel    =   -2147483638
      GridColor       =   -2147483630
      AllowBigSelection=   0   'False
      GridLinesFixed  =   3
      ScrollBars      =   0
   End
   Begin VB.CommandButton cmdAD1 
      Caption         =   "füllen"
      Height          =   252
      Left            =   840
      TabIndex        =   2
      Top             =   120
      Width           =   852
   End
   Begin MSFlexGridLib.MSFlexGrid MSFlexAD2 
      Height          =   1455
      Left            =   480
      TabIndex        =   1
      Top             =   2040
      Width           =   6495
      _ExtentX        =   11456
      _ExtentY        =   2566
      _Version        =   393216
      AllowBigSelection=   0   'False
      Enabled         =   -1  'True
      GridLinesFixed  =   1
      ScrollBars      =   0
   End
   Begin MSFlexGridLib.MSFlexGrid MSFlexAD1 
      Height          =   972
      Left            =   480
      TabIndex        =   0
      Top             =   480
      Width           =   6492
      _ExtentX        =   11456
      _ExtentY        =   1720
      _Version        =   393216
      AllowBigSelection=   0   'False
      Enabled         =   0   'False
      HighLight       =   0
      GridLinesFixed  =   1
      ScrollBars      =   0
      BorderStyle     =   0
   End
End
Attribute VB_Name = "frmAntennenausdruck"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'=========================================================
' Formular zum Ausdrucken der Antennendaten
'
'
'
' 12.09.2004 / HB9ZS
'=========================================================
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


Private Sub cmdAD1_Click()

    füllen

End Sub



Private Sub füllen()


'Flexgrid Kopieren

    Dim Reihe As Integer
    Dim Spalte As Integer
 
 'FlexGrid leeren
    MSFlexAD1.Clear
    MSFlexAD2.Clear
 
 'Flexgrid Kopf
    MSFlexAD1.Row = 1
    MSFlexAD1.Col = 1
    MSFlexAD1.CellFontBold = True
    MSFlexAD1.CellFontSize = 15
    MSFlexAD1.TextMatrix(1, 1) = LoadResString(1526 + RS)
 

    'Name, Adresse
    MSFlexAD1.TextMatrix(3, 1) = frmMain.txt03
    MSFlexAD1.TextMatrix(4, 1) = frmMain.txt04
    MSFlexAD1.TextMatrix(5, 1) = frmMain.txt05
    'Hersteller
    MSFlexAD1.TextMatrix(7, 1) = LoadResString(1503 + RS) & ":  " & frmMain.flexA1.TextMatrix(1, 1)
    'Antennentyp
    MSFlexAD1.TextMatrix(8, 1) = LoadResString(1525 + RS) & "  " & frmMain.flexA1.TextMatrix(1, 2)
    
    Dim X As Integer
    Dim A As String
    Dim Pos As Integer
    
    'Horizontal drehbar
    If frmMain.txtA10.Text = "" Then
        Pos = InStr(LoadResString(1752 + RS), "(")
        X = Pos
        A = Left(LoadResString(1752 + RS), (X - 2))
        MSFlexAD1.TextMatrix(10, 1) = A & ": " & frmMain.txtA8.Text
    
    ElseIf frmMain.txtA10.Text = 0 Then
        Pos = InStr(LoadResString(1752 + RS), "(")
        X = Pos
        A = Left(LoadResString(1752 + RS), (X - 2))
        MSFlexAD1.TextMatrix(10, 1) = A & ": " & frmMain.txtA8.Text
    
    Else
        Pos = InStr(LoadResString(1752 + RS), "(")
        X = Pos
        A = Left(LoadResString(1752 + RS), (X - 2))
        MSFlexAD1.TextMatrix(10, 1) = A & ": " & frmMain.txtA8.Text & "   " & LoadResString(1754 + RS) & "  " & frmMain.txtA10.Text & " " & LoadResString(1755 + RS)
    
    End If
    
    'Vertikal drehbar
    If frmMain.txtA11.Text = "" Then
        Pos = InStr(LoadResString(1753 + RS), "(")
        X = Pos
        A = Left(LoadResString(1753 + RS), (X - 2))
        MSFlexAD1.TextMatrix(11, 1) = A & ": " & frmMain.txtA9.Text
    
    ElseIf frmMain.txtA11.Text = 0 Then
        Pos = InStr(LoadResString(1753 + RS), "(")
        X = Pos
        A = Left(LoadResString(1753 + RS), (X - 2))
        MSFlexAD1.TextMatrix(11, 1) = A & ": " & frmMain.txtA9.Text
    
    Else
        Pos = InStr(LoadResString(1753 + RS), "(")
        X = Pos
        A = Left(LoadResString(1753 + RS), (X - 2))
        MSFlexAD1.TextMatrix(11, 1) = A & ": " & frmMain.txtA9.Text & "   " & LoadResString(1754 + RS) & "  " & frmMain.txtA11.Text & " " & LoadResString(1755 + RS)
    
    End If
    
    
    'Winkeldämfungs Anzeige
    '========================
    'Eingeblendeter Text in Fettschrift
    
    For X = 1 To 2
    MSFlexAD4.Row = X
    MSFlexAD4.Col = 1
    MSFlexAD4.CellFontBold = True
    Next X
        
    '1. Zeile
    '    MSFlexAD4.TextMatrix(1, 1) = LoadResString(1529 + RS) & ":  " & frmMain.txtA7.Text & " " & LoadResString(1755 + RS) & " :  " & frmMain.cboA1.List(frmMain.cboA1.ListIndex) & " MHz"
    MSFlexAD4.TextMatrix(1, 1) = LoadResString(1529 + RS) & ":  " & frmMain.cboA1.List(frmMain.cboA1.ListIndex) & " MHz"
    
    '2. Zeile
        
    If No_Diagram = 1 Then
        
        If RS = 0 Then
            MSFlexAD4.TextMatrix(2, 1) = LoadResString(1519 + RS)  '"Kein vertikales Strahlungs-Diagramm für diese Frequenz vorhanden"
        End If
        
        If RS = 1000 Then
            MSFlexAD4.TextMatrix(2, 1) = LoadResString(1519 + RS)   'französisch
        End If
        
        If RS = 2000 Then
            MSFlexAD4.TextMatrix(2, 1) = LoadResString(1519 + RS)   'italienisch
        End If

        
    ElseIf No_Diagram = 0 Then  'Text löschen
    
        MSFlexAD4.TextMatrix(2, 1) = ""
   
    End If
    
    
    MSFlexAD1.ColAlignment(1) = 2
 
    Dim xc As Integer
    For xc = 7 To 8
    MSFlexAD1.Row = xc
    MSFlexAD1.Col = 1
    MSFlexAD1.CellFontBold = True
    Next

 
 'Flexgrid Antennendaten
 
    'Löschen
    For Reihe = 0 To 14
        For Spalte = 0 To 14
        
                MSFlexAD2.TextMatrix(Reihe, Spalte) = ""
 
        Next Spalte
        
    Next Reihe

 
    'Kopieren
    For Reihe = 0 To 14
        For Spalte = 0 To 14
        
                MSFlexAD2.TextMatrix(Reihe, Spalte) = frmMain.flexA1.TextMatrix(Reihe, Spalte)
 
        Next Spalte
        
    Next Reihe
 
    'Erste Spalte schmal einstellen.
    MSFlexAD2.ColAlignment(0) = 1   ' Zentrieren.
    MSFlexAD2.ColAlignment(2) = 0   ' links

    'Titelleiste fett schreiben
    'Dim xc As Integer
    For xc = 1 To 14
    MSFlexAD2.Row = 0
    MSFlexAD2.Col = xc
    MSFlexAD2.CellFontBold = True
    Next

    MSFlexAD2.Col = 0
    MSFlexAD2.Row = 14
    
End Sub


Private Sub Form_Load()

 'Flexgrid Kopf
    
    MSFlexAD1.BorderStyle = 0
    
    'MSFlexAD1.Height = 2000
    'MSFlexAD1.Width = 9475
    MSFlexAD1.Cols = 2
    MSFlexAD1.Rows = 13
    MSFlexAD1.GridLines = flexGridNone
    MSFlexAD1.GridLinesFixed = flexGridNone
      
    MSFlexAD1.ColWidth(0) = 0
    MSFlexAD1.ColWidth(1) = 4500
    MSFlexAD1.RowHeight(0) = 0
    MSFlexAD1.RowHeight(1) = 500
    MSFlexAD1.Width = MSFlexAD1.ColWidth(0) + MSFlexAD1.ColWidth(1)
    MSFlexAD1.Height = MSFlexAD1.RowHeight(2) + ((MSFlexAD1.Rows - 1) * MSFlexAD1.RowHeight(2)) + 50
    MSFlexAD1.Enabled = False

'Flexgrid Antennendaten
    MSFlexAD2.Height = 3690
    MSFlexAD2.Width = 9475
    MSFlexAD2.Rows = 15
    MSFlexAD2.Cols = 15
    MSFlexAD2.Height = MSFlexAD2.RowHeight(1) * MSFlexAD2.Rows + 50
    MSFlexAD2.Top = MSFlexAD1.Top + MSFlexAD1.Height + 500
    MSFlexAD2.Enabled = False
    
    MSFlexAD2.ColWidth(0) = 0
    MSFlexAD2.ColWidth(1) = 1600
    MSFlexAD2.ColWidth(2) = 1100
    MSFlexAD2.ColWidth(3) = 600
    MSFlexAD2.ColWidth(4) = 700
    MSFlexAD2.ColWidth(5) = 550
    MSFlexAD2.ColWidth(6) = 550
    MSFlexAD2.ColWidth(7) = 550
    MSFlexAD2.ColWidth(8) = 550
    MSFlexAD2.ColWidth(9) = 550
    MSFlexAD2.ColWidth(10) = 550
    MSFlexAD2.ColWidth(11) = 550
    MSFlexAD2.ColWidth(12) = 550
    MSFlexAD2.ColWidth(13) = 550
    MSFlexAD2.ColWidth(14) = 550
    
'Zellen Focus(kein Rahmen)Farbe in Eigenschaften festlegen
    MSFlexAD2.FocusRect = 0

'FlexGrid3
    MSFlexAD3.Height = 200
    MSFlexAD3.Width = 9475
    MSFlexAD3.Rows = 2
    MSFlexAD3.Cols = 6
    MSFlexAD3.RowHeight(0) = 10
    MSFlexAD3.RowHeight(1) = 500
    MSFlexAD3.Height = MSFlexAD3.RowHeight(1) + 30
    MSFlexAD3.Top = MSFlexAD2.Top - MSFlexAD3.Height
    MSFlexAD3.Enabled = False
    
    MSFlexAD3.ColWidth(0) = 0
    MSFlexAD3.ColWidth(1) = 1600
    MSFlexAD3.ColWidth(2) = 1100
    MSFlexAD3.ColWidth(3) = 600
    MSFlexAD3.ColWidth(4) = 700
    MSFlexAD3.ColWidth(5) = 10 * 550 + 50


    MSFlexAD3.WordWrap = True
    MSFlexAD3.TextMatrix(1, 4) = LoadResString(1524 + RS)

    MSFlexAD3.TextMatrix(1, 5) = LoadResString(1523 + RS)
    MSFlexAD3.ColAlignment(5) = flexAlignCenterCenter

'FlexGrid4
    
    MSFlexAD4.BorderStyle = 0
    MSFlexAD4.GridLines = flexGridNone
    MSFlexAD4.GridLinesFixed = flexGridNone

    MSFlexAD4.Height = 200
    MSFlexAD4.Width = 9475
    MSFlexAD4.Rows = 3
    MSFlexAD4.Cols = 2
    MSFlexAD4.RowHeight(0) = 0
    MSFlexAD4.RowHeight(1) = 500
    MSFlexAD4.RowHeight(2) = 300
    MSFlexAD4.Height = MSFlexAD4.RowHeight(1) + MSFlexAD4.RowHeight(2) '+ 30
    MSFlexAD4.Top = MSFlexAD2.Top + MSFlexAD2.Height + 200
    MSFlexAD4.Enabled = False
    
    MSFlexAD4.ColWidth(0) = 0
    MSFlexAD4.ColWidth(1) = 9480
  '  MSFlexAD4.ColWidth(2) = 5500
  '  MSFlexAD4.ColWidth(3) = 1000
    MSFlexAD4.ColAlignment(1) = flexAlignCenterCenter
    

End Sub



'===============

Public Sub Antennendaten_drucken()
    
    
'Druckvorgang in Statuszeile anzeigen
    frmMain1.StatusBar1.Panels(1) = "  Printer activ"
    
'Winkel-Diagramm erstellen
    frmS2.mnu_erstellen_Click

'Formular füllen
    füllen
    
'Formular drucken

    Dim tRange      As TFormatRange
    Dim lReturn     As Long
    Dim LeftMargin As Integer
    Dim TopMargin As Integer
    Dim RightMargin As Integer
    Dim BottomMargin As Integer

    'Blattformat A4 hoch
        
    Printer.Orientation = vbPRORPortrait
    

    '=== MSFlexAD1 ==================================================

    lReturn = SendMessage(MSFlexAD1.hwnd, VP_FORMATRANGE, 1, 0)

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
            .rc.Top = Printer.ScaleY(TopMargin + 20, vbMillimeters)  '10, vbMillimeters)
            .rc.Right = .rcPage.Right - Printer.ScaleX(RightMargin, vbMillimeters)
            .rc.Bottom = .rcPage.Bottom - Printer.ScaleY(BottomMargin, vbMillimeters)
        End With

        'Drucker initialisieren
        Printer.Print vbNullString

        'Seite(n) drucken
        Do
            lReturn = SendMessage(MSFlexAD1.hwnd, VP_FORMATRANGE, 0, VarPtr(tRange))
            If lReturn < 0 Then
                Exit Do
            Else
                Printer.NewPage
            End If
        Loop
        
        lReturn = SendMessage(MSFlexAD1.hwnd, VP_FORMATRANGE, 0, 0)

    End If


     '=== Label1 ==================================================

    lReturn = SendMessage(MSFlexAD3.hwnd, VP_FORMATRANGE, 1, 0)

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
            .rc.Top = Printer.ScaleY(TopMargin + 82, vbMillimeters)  '10, vbMillimeters)
            .rc.Right = .rcPage.Right - Printer.ScaleX(RightMargin, vbMillimeters)
            .rc.Bottom = .rcPage.Bottom - Printer.ScaleY(BottomMargin, vbMillimeters)
        End With

        'Drucker initialisieren
        Printer.Print vbNullString

        'Seite(n) drucken
        Do
            lReturn = SendMessage(MSFlexAD3.hwnd, VP_FORMATRANGE, 0, VarPtr(tRange))
            If lReturn < 0 Then
                Exit Do
            Else
                Printer.NewPage
            End If
        Loop
        
        lReturn = SendMessage(MSFlexAD3.hwnd, VP_FORMATRANGE, 0, 0)

    End If
   
    
    
    '==== MSFlexAD2 ==================================================

    lReturn = SendMessage(MSFlexAD2.hwnd, VP_FORMATRANGE, 1, 0)

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
            .rc.Top = Printer.ScaleY(TopMargin + 90, vbMillimeters)
            .rc.Right = .rcPage.Right - Printer.ScaleX(RightMargin, vbMillimeters)
            .rc.Bottom = .rcPage.Bottom - Printer.ScaleY(BottomMargin, vbMillimeters)
        End With

        'Drucker initialisieren
        Printer.Print vbNullString

        'Seite(n) drucken
        Do
            lReturn = SendMessage(MSFlexAD2.hwnd, VP_FORMATRANGE, 0, VarPtr(tRange))
            If lReturn < 0 Then
                Exit Do
            Else
                Printer.NewPage
            End If
        Loop

        lReturn = SendMessage(MSFlexAD2.hwnd, VP_FORMATRANGE, 0, 0)

    End If

        
    '==== MSFlexAD4 ==================================================

    lReturn = SendMessage(MSFlexAD4.hwnd, VP_FORMATRANGE, 1, 0)

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
            .rc.Top = Printer.ScaleY(TopMargin + 158, vbMillimeters)
            .rc.Right = .rcPage.Right - Printer.ScaleX(RightMargin, vbMillimeters)
            .rc.Bottom = .rcPage.Bottom - Printer.ScaleY(BottomMargin, vbMillimeters)
        End With

        'Drucker initialisieren
        Printer.Print vbNullString

        'Seite(n) drucken
        Do
            lReturn = SendMessage(MSFlexAD4.hwnd, VP_FORMATRANGE, 0, VarPtr(tRange))
            If lReturn < 0 Then
                Exit Do
            Else
                Printer.NewPage
            End If
        Loop

        lReturn = SendMessage(MSFlexAD2.hwnd, VP_FORMATRANGE, 0, 0)

    End If

        

    If No_Diagram = 0 Then   ' Diagramm vorhanden für Antennendatenausdruck

        'Diagramm drucken
        frmS2.Diagramm Printer
    
    Else
    
    End If
    
    Printer.EndDoc
    
    Printer.Orientation = vbPRORPortrait


'Statusanzeige löschen
    frmMain1.StatusBar1.Panels(1) = ""

End Sub


