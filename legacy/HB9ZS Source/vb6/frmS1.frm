VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form frmS1 
   BackColor       =   &H80000005&
   Caption         =   "1520"
   ClientHeight    =   6960
   ClientLeft      =   165
   ClientTop       =   450
   ClientWidth     =   10005
   Icon            =   "frmS1.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6960
   ScaleWidth      =   10005
   Begin MSFlexGridLib.MSFlexGrid flexS2 
      Height          =   3375
      Left            =   8280
      TabIndex        =   0
      Top             =   240
      Visible         =   0   'False
      Width           =   615
      _ExtentX        =   1085
      _ExtentY        =   5953
      _Version        =   393216
   End
   Begin MSFlexGridLib.MSFlexGrid flexS1 
      Height          =   8415
      Left            =   8880
      TabIndex        =   1
      Top             =   240
      Visible         =   0   'False
      Width           =   1815
      _ExtentX        =   3201
      _ExtentY        =   14843
      _Version        =   393216
   End
   Begin VB.Label lblS2 
      Caption         =   "0"
      Height          =   255
      Left            =   7800
      TabIndex        =   6
      Top             =   0
      Visible         =   0   'False
      Width           =   615
   End
   Begin VB.Label Label1 
      Caption         =   "Label1"
      Height          =   255
      Left            =   9360
      TabIndex        =   5
      Top             =   0
      Visible         =   0   'False
      Width           =   495
   End
   Begin VB.Label lblS1 
      BackColor       =   &H80000005&
      Caption         =   "Hersteller"
      Height          =   255
      Left            =   360
      TabIndex        =   4
      Top             =   120
      Width           =   1935
   End
   Begin VB.Label lblS3 
      Alignment       =   1  'Rechts
      BackColor       =   &H80000005&
      Caption         =   "Frequenz"
      Height          =   255
      Left            =   2520
      TabIndex        =   3
      Top             =   120
      Width           =   2175
   End
   Begin VB.Label lblS5 
      Alignment       =   1  'Rechts
      BackColor       =   &H80000005&
      Caption         =   "Gewinn"
      Height          =   255
      Left            =   4920
      TabIndex        =   2
      Top             =   120
      Width           =   2775
   End
   Begin VB.Menu mnu_zeichnen 
      Caption         =   "1521"
   End
   Begin VB.Menu mnu_close 
      Caption         =   "1522"
   End
End
Attribute VB_Name = "frmS1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'================================================
'Diagramm Fenster. In diesem Fenster werden die
'Winkeldaten in ein Kreisdiagramm gezeichnet.
' 28. 09. 2003  hb9zs
'================================================

Option Explicit

Private Sub Form_Load()

    frmS1.Left = GetSetting(App.Title, "Settings", "frmS1Left", 1000)
    frmS1.Top = GetSetting(App.Title, "Settings", "frmS1Top", 1000)
    frmS1.Width = GetSetting(App.Title, "Settings", "frmS1Width", 6500)
    frmS1.Height = GetSetting(App.Title, "Settings", "frmS1Height", 6500)

    ' Texte festlegen
    frmS1.Caption = LoadResString(1520 + RS)
    mnu_zeichnen.Caption = LoadResString(1521 + RS)
    mnu_close.Caption = LoadResString(1522 + RS)

'Flexgid S1 spezifizieren
    flexS1.Rows = 38
    flexS1.Cols = 4
    flexS1.ColWidth(0) = 100
    flexS1.ColWidth(1) = 500
    flexS1.ColWidth(2) = 500
    flexS1.ColWidth(3) = 500
    flexS1.Row = 0
    flexS1.Col = 1
    flexS1.Text = "Wink"
    flexS1.Col = 2
    flexS1.Text = "dB"

'Flexgid S2 spezifizieren
    flexS2.Rows = 11
    flexS2.Cols = 2
    flexS2.ColWidth(0) = 100
    flexS2.ColWidth(1) = 500

'1.Kolonne mit 0 - 360 belegen
    Dim X As Integer

    flexS1.Col = 1
    For X = 0 To 36
        flexS1.Row = X + 1
        flexS1.Text = X * 10
    Next

End Sub



Sub Diagramm(obj As Object)
On Error Resume Next
  Refresh          'Altes Diagramm löschen

  Dim r As Double  'Radius
  Dim s As Double  'Strahl
  Dim s1 As Double
  Dim u As Double
  Dim pb As Double 'pi /36 Bogenteil
  Dim W As Double
  Dim W1 As Double
  Dim x1 As Double
  Dim y1 As Double
  Dim X2 As Double
  Dim y2 As Double
  
  'Radius an Dämpfung der Antenne anpassen (max. db + 10)
  r = Round(flexS2.TextMatrix(10, 1) / 10, 0)
  r = (r * 10 + 10)
  
  Dim cor As Double
  Dim st As Double
  Dim st1 As Double
  If r > 50 Then     'Korrektur der Skalierung wenn Dämpfung
                     ' grösser 50 db
    cor = 0.3       'Korrektur der Diagrammgrösse
    st = 20         'Korrektur der Anzahl Kreise (Step 20)
    st1 = 10        'Korrektur des Beschriftungsabstand um den Kreis (mm)
  Else
    cor = 1         'Keine Korrektur der Diagrammgrösse
    st = 10         'Anzahl Kreise (Step 10)
    st1 = 5         'Beschriftungsabstand um den Kreis (mm)
  End If
 
 Dim X As Single
 
  'obj.ScaleMode = vbMillimeters          ' Millimeter als Maßeinheit
  obj.ScaleMode = cor * vbMillimeters  'verkleinerung Millimeter als Maßeinheit

  obj.ScaleLeft = -obj.ScaleWidth / 2     ' Ursprung in die Mitte minus 20 Milimeter
  obj.ScaleTop = -obj.ScaleHeight / 2
  
  
  For X = 10 To r Step 10  ' Radius X in 10 Schritten
    
        obj.Circle (0, 0), X, vbBlack           ' Kreise drum herum
  
  Next X
  
 
  ' Beschriftung
  'obj.FontName = "Courier New"       ' Schrift für Beschriftung
  obj.FontName = "Arial"              ' Schrift für Beschriftung

  obj.FontSize = 8.5
   
  'For x = 0 To r Step 10
  For X = 0 To r Step st   'Verkleinerung bei grösser 50db
    
    ' Skala horizontal
        obj.CurrentX = X - obj.TextWidth(CStr(X)) / 2
        obj.CurrentY = 0
        obj.Print CStr(r - X)
  Next X
 
   
  'For x = -r To 0 Step 10
  For X = -r To 0 Step st  'Verkleinerung bei grösser 50db
    ' Skala horizontal
        obj.CurrentX = X - obj.TextWidth(CStr(X)) / 2
        obj.CurrentY = 0
        obj.Print CStr(X + r)
    ' Skala vertikal
     'obj.CurrentX = -obj.TextWidth(CStr(x))
    ' obj.CurrentY = x - obj.TextHeight(CStr(x)) / 2
    ' obj.Print CStr(x)
  Next X
  
  
  'Kreis Kreissegmente zeichnen (Strahlen) 0 - 360 Grad
  
  pb = 0.1745        'pi/36
  
  For s = 0 To 35 Step 1    '36 Teile
        u = s * pb         'Bogenmass des einzelnen Strahls
  
  
        ' Radienzeichnen Dämpfungskreise
        ' ==============================
        
        obj.Circle (0, 0), r, vbBlack, 0, -(6.28 - u) 'Schwarze Zahlen
  
        

  Next s
   
  
        ' Tabellenwert als Kreislein eintragen
        ' ====================================
    
  Const pi = 3.14159265358979
  
  '0 - 90 Grad   (für 360 Grad s max = 35)
  For s = 0 To 9 Step 1     '36 Teile
        DrawWidth = 1       'Strichdicke
        W = flexS1.TextMatrix(s + 1, 2) ' Tabellenwert
        W = r - W   'Kreislein = Radius - Tabellenwert
        obj.Circle (W * Cos(s * pi / 18), -W * Sin(s * pi / 18)), 0.5, vbRed    '  Kreislein zeichnen
 
  Next s
  
  '270 - 360 Grad
  For s = 27 To 35 Step 1
        W = flexS1.TextMatrix(s + 1, 2) ' Tabellenwert
        W = r - W   'Kreislein = Radius - Tabellenwert
        obj.Circle (W * Cos(s * pi / 18), -W * Sin(s * pi / 18)), 0.5, vbRed    '  Kreislein zeichnen
 
  Next s

  
 
        ' Beschriftung 0 - 360 Grad
        ' =========================
 
  For s = 0 To 35 Step 3  ' in 30 Grad schritten
        u = s * pb         'Bogenmass des einzelnen Strahls

        
 
        obj.CurrentX = ((r + st1) + obj.TextWidth(CStr(s * 10)) / 2) * Cos(s * pi / 18) - obj.TextWidth(CStr(s * 10)) / 2
        obj.CurrentY = -(r + st1) * Sin(s * pi / 18) - 1
        obj.Print CStr((36 - s) * 10)  'uhrzeigersinn beschriften Gradteilung 0-360
        'obj.Print CStr(s * 10)
  
  Next s

   
     
   ' Tabellenwert in Diagramm eintragen
   '===================================
   
   '0 - 90 Grad
 For s = 1 To 9 Step 1 '36 Step 1
   
      W = flexS1.TextMatrix(s + 1, 2) 'Tabellenwert
      W = r - W    ' Wert im Kreis = Radius - Tabellenwert
     
      s1 = s - 1     'letzter Tabellenwert
      '-----------------
      W1 = flexS1.TextMatrix(s, 2)   ' Tabellenwert W-1 letzter
      W1 = r - W1
      x1 = W * Cos(s * pi / 18)       'Position des Kreisleins
      y1 = -W * Sin(s * pi / 18)      'Position des Kreisleins
     
      X2 = W1 * Cos(s1 * pi / 18)    'Position des Kreisleins
      y2 = -W1 * Sin(s1 * pi / 18)   'Position des Kreisleins
     
      obj.DrawWidth = 2              'Strichdicke
      obj.Line (X2, y2)-(x1, y1), vbBlue     'Linie zeichnen
    
  Next s
  '======
  
 ' 270-360 Grad
 For s = 10 To 18 Step 1 '36 Step 1
   
      W = flexS1.TextMatrix(s + 1, 2) 'Tabellenwert
      W = r - W ' Wert im Kreis = Radius - Tabellenwert
     
      s1 = s - 1     'letzter Tabellenwert
      '-----------------
      W1 = flexS1.TextMatrix(s, 2)   ' Tabellenwert W-1 letzter
      W1 = r - W1     ' Wert im Kreis = Radius - Tabellenwert
      x1 = -W * Cos(s * pi / 18)       'Position des Kreisleins
      y1 = W * Sin(s * pi / 18)      'Position des Kreisleins
     
      X2 = -W1 * Cos(s1 * pi / 18)    'Position des Kreisleins
      y2 = W1 * Sin(s1 * pi / 18)   'Position des Kreisleins
     
      obj.Line (X2, y2)-(x1, y1), vbBlue     'Linie zeichnen
    
  Next s
  
  
  
  '===
'  180 - 270 Grad
'  For s = 19 To 27 Step 1 '36 Step 1
   
'      W = flexS1.TextMatrix(s + 1, 2) 'Tabellenwert
'      W = r - W      ' Wert im Kreis = Radius - Tabellenwert
      
'      s1 = s - 1     'letzter Tabellenwert
      '-----------------
'      W1 = flexS1.TextMatrix(s, 2)   ' Tabellenwert W-1 letzter
'      W1 = r - W1    ' Wert im Kreis = Radius - Tabellenwert
'      x1 = W * Cos(s * pi / 18)       'Position des Kreisleins
'      y1 = -W * Sin(s * pi / 18)      'Position des Kreisleins
     
'      X2 = W1 * Cos(s1 * pi / 18)    'Position des Kreisleins
'      y2 = -W1 * Sin(s1 * pi / 18)   'Position des Kreisleins
     
'      obj.Line (X2, y2)-(x1, y1), vbBlue     'Linie zeichnen
    
'  Next s
  '===
' 90 - 180 Grad
'  For s = 28 To 36 Step 1 '36 Step 1
   
'      W = flexS1.TextMatrix(s + 1, 2) 'Tabellenwert
'      W = r - W      ' Wert im Kreis = Radius - Tabellenwert
     
'      s1 = s - 1     'letzter Tabellenwert
      '-----------------
'      W1 = flexS1.TextMatrix(s, 2)   ' Tabellenwert W-1 letzter
'      W1 = r - W1      ' Wert im Kreis = Radius - Tabellenwert
'      x1 = -W * Cos(s * pi / 18)       'Position des Kreisleins
'      y1 = W * Sin(s * pi / 18)      'Position des Kreisleins
     
'      X2 = -W1 * Cos(s1 * pi / 18)    'Position des Kreisleins
'      y2 = W1 * Sin(s1 * pi / 18)   'Position des Kreisleins
     
'      obj.Line (X2, y2)-(x1, y1), vbBlue     'Linie zeichnen
    
'  Next s
  '====
  
End Sub


'Private Sub Command1_Click()
Private Sub lblS2_change()


 Dim X As Integer
 Dim x1 As Integer
 Dim y1 As Integer
 Dim y2 As Integer
 Dim y3 As Integer
 Dim co As Double

    For X = 0 To 9
    
      '  flexS1.Col = 2          'Flexgrid Werte eintragen
        
      '  flexS1.Row = x + 1  ' Hier später löschen Werte aus Antennendaten übernehmen
      '  flexS1.Text = 4 * x   'Wert = 4* Radiusangabe z.B 4* 2 (für 20 Grad)
   
        flexS2.Col = 1          'Flexgrid Werte eintragen
        flexS2.Row = X + 1
        flexS2.Text = flexS1.TextMatrix(X + 1, 2)
    
    Next
   
    For X = 1 To 10             ' erste zehn Werte Eintragen 0 - 90 Grad
    
        Label1.Caption = flexS1.TextMatrix(X, 2)
    
    y1 = 20 - (X)              'Werte in 180 - 100 Grad eintragen
        flexS1.Col = 2
        flexS1.Row = y1
        flexS1.Text = Label1.Caption
        
    y2 = 19 + X - 1            'Werte 190 - 270 Grad eintragen
        flexS1.Col = 2
        flexS1.Row = y2
        flexS1.Text = Label1.Caption
        
    y3 = 38 - (X)             ' Werte 360 - 280 Grad eintragen
        flexS1.Col = 2
        flexS1.Row = y3
        flexS1.Text = Label1.Caption
    
    Next
    
    
    flexS2.Sort = flexS2.Col 'FlexS2 sortieren für Maximalwert von Radius
        
        
  Diagramm Me       'Sub Programm laden
  
End Sub


 Private Sub mnu_zeichnen_Click()
 
  Diagramm Me
  
End Sub

Private Sub Form_Resize()

'Demo Me
 Refresh
 
End Sub


Private Sub mnu_close_Click()

Unload Me

End Sub

Private Sub mnu_print_Click()

Diagramm Printer
Printer.EndDoc

End Sub

Private Sub Form_Unload(Cancel As Integer)
    Dim i As Integer
    

    If Me.WindowState <> vbMinimized Then
        SaveSetting App.Title, "Settings", "frmS1Left", frmS1.Left
        SaveSetting App.Title, "Settings", "frmS1Top", frmS1.Top
        SaveSetting App.Title, "Settings", "frmS1Width", frmS1.Width
        SaveSetting App.Title, "Settings", "frmS1Height", frmS1.Height
    End If
    
End Sub



