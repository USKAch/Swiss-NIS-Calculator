VERSION 5.00
Object = "{0ECD9B60-23AA-11D0-B351-00A0C9055D8E}#6.0#0"; "MSHFLXGD.OCX"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Begin VB.Form frmTyp 
   Caption         =   "Eigabemaske für Antennentypen"
   ClientHeight    =   6855
   ClientLeft      =   885
   ClientTop       =   1245
   ClientWidth     =   11250
   Icon            =   "frmTyp.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6855
   ScaleWidth      =   11250
   Begin VB.TextBox txtT2 
      Height          =   285
      Left            =   120
      TabIndex        =   9
      Top             =   6240
      Visible         =   0   'False
      Width           =   5655
   End
   Begin VB.CommandButton cmd1 
      Caption         =   "markierter Antennentyp laden"
      Height          =   735
      Left            =   2520
      TabIndex        =   8
      Top             =   1920
      Width           =   1095
   End
   Begin MSComctlLib.StatusBar StatusBar1 
      Align           =   2  'Unten ausrichten
      Height          =   255
      Left            =   0
      TabIndex        =   6
      Top             =   6600
      Width           =   11250
      _ExtentX        =   19844
      _ExtentY        =   450
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   2
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            AutoSize        =   1
            Object.Width           =   17886
         EndProperty
         BeginProperty Panel2 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Style           =   5
            Object.Width           =   1411
            MinWidth        =   1411
            TextSave        =   "10:02"
         EndProperty
      EndProperty
   End
   Begin VB.TextBox txtT1 
      Height          =   285
      Left            =   600
      TabIndex        =   4
      Text            =   "txtT1"
      Top             =   5160
      Visible         =   0   'False
      Width           =   1095
   End
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   2160
      Top             =   840
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.TextBox txtEditT1 
      Height          =   300
      Left            =   2280
      TabIndex        =   2
      Text            =   "txtEditT1"
      Top             =   5160
      Visible         =   0   'False
      Width           =   1455
   End
   Begin VB.TextBox txtT3 
      Height          =   285
      Left            =   120
      TabIndex        =   1
      Text            =   "TxtT3"
      Top             =   5880
      Visible         =   0   'False
      Width           =   5535
   End
   Begin VB.TextBox TextBoxT1 
      Height          =   285
      Left            =   120
      TabIndex        =   0
      Text            =   "TextBoxT1"
      Top             =   5520
      Visible         =   0   'False
      Width           =   5535
   End
   Begin MSHierarchicalFlexGridLib.MSHFlexGrid flexT1 
      Height          =   4335
      Left            =   360
      TabIndex        =   3
      Top             =   720
      Width           =   1815
      _ExtentX        =   3201
      _ExtentY        =   7646
      _Version        =   393216
      Appearance      =   0
      _NumberOfBands  =   1
      _Band(0).Cols   =   2
   End
   Begin VB.Label lbl2 
      Caption         =   "Muster"
      Height          =   255
      Left            =   2400
      TabIndex        =   7
      Top             =   4680
      Visible         =   0   'False
      Width           =   1095
   End
   Begin VB.Label lbl1 
      Height          =   495
      Left            =   240
      TabIndex        =   5
      Top             =   120
      Width           =   2055
   End
   Begin VB.Menu mnuDatei 
      Caption         =   "Datei"
      Begin VB.Menu mnu_Deltyp 
         Caption         =   "markierter Antennentyp löschen"
      End
      Begin VB.Menu mnu_Beenden 
         Caption         =   "Fenster schliessen"
      End
   End
End
Attribute VB_Name = "frmTyp"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'=======================================
'Antennentypen definieren
'
'18.03.2003 / hb9zs
'=======================================

Public clsFGEdit As clsFlexGridEdit

Option Explicit

Private Ascii As String
Private Const WM_USER = &H400
Private Const VP_FORMATRANGE = WM_USER + 125
Private Const VP_YESIDO = 456654

'yT Def für Flexgrid Row (für Klassenmodul)
Private YT As Long
Private YH As Long

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


Private Sub Ant_laden()

If flexT1.Row > 1 Then

    lbl2.Caption = "Eigene\" & flexT1.TextMatrix(flexT1.Row, 2)

    frmAntEingabe.von_Typen_laden
End If

End Sub

Private Sub cmd1_Click()
    Ant_laden
End Sub

Private Sub Form_Load()

  ' Typen Eingabe Flex T1
    
      'Laden der Einstellungen aus Registry
    Me.Left = GetSetting(App.Title, "Settings", "TypLeft", 1000)
    Me.Top = GetSetting(App.Title, "Settings", "TypTop", 1000)
    Me.Width = GetSetting(App.Title, "Settings", "TypWidth", 2670)
    Me.Height = GetSetting(App.Title, "Settings", "TypHeight", 6210)
  
    
  'Texte laden
    frmTyp.Caption = LoadResString(1401 + RS)
    
    mnuDatei.Caption = LoadResString(1402 + RS)
'    mnu_Typeload.Caption = LoadResString(1403 + RS)
'    mnu_Typesave.Caption = LoadResString(1404 + RS)
'    mnuAdtype.Caption = LoadResString(1405 + RS)       'nicht mehr vorhanden
    mnu_Deltyp.Caption = LoadResString(1406 + RS)
    mnu_Beenden.Caption = LoadResString(1407 + RS)
    lbl1.Caption = LoadResString(1411 + RS)
    
  ' Speicherungspfad definieren
    txtT3.Text = App.Path & "\Ant_Dat" & "\" & "19_Eigene.typ"
      
    flexT1.Rows = 2
    flexT1.Cols = 3
    flexT1.ColWidth(0) = 0
    flexT1.ColWidth(1) = 300
    flexT1.ColWidth(2) = 1200
   
  ' Zellen Focus(kein Rahmen)Farbe in Eigenschaften festlegen
    flexT1.FocusRect = 0
    

  ' Flexgrid füllen

  ' Text : Row 0, Col 1
    flexT1.TextMatrix(0, 1) = "Nr"
    flexT1.TextMatrix(0, 2) = "Typen"
    
        
  ' Kolonne ausrichten .
    flexT1.ColAlignment(2) = 0 'links
  ' Bearbeitungsfeld initialisieren (so daß es jetzt geladen wird).
    txtEditT1.Text = ""  'Textfeld leeren
    
    Antennentypenliste_laden    'Typen vom Disk laden
   
    
End Sub


Private Sub Form_Resize()

    On Error Resume Next
    
    If frmTyp.Height > 6210 Then
        frmTyp.Height = 6210
    End If
    
    If frmTyp.ScaleWidth > 6100 Then
        frmTyp.Width = 6100
    End If
     
End Sub



Private Sub flexT1_Click()

' Tabelle Typen Editieren
'Ant_laden  'Sub
'Aufruf des Klassen Moduls
Set clsFGEdit = New clsFlexGridEdit
Set clsFGEdit.FlexGridControl = flexT1

End Sub


    
Private Sub Antennentypenliste_laden1()

'File vom Disk laden mit Common Dialog Box
On Error Resume Next
            
    CommonDialog1.CancelError = True                'CancelError auf True setzen
    On Error GoTo ErrHandler
    CommonDialog1.Flags = cdlOFNHideReadOnly        'Attribute setzen
    CommonDialog1.Filter = "Flat File (*.typ)|*.typ|All Files (*.*)|*.*"
    CommonDialog1.FilterIndex = 1                   'bei Dateityp, wird *.shw angeboten
    CommonDialog1.ShowOpen                          'Dialogfeld "Öffnen" anzeigen
                        
    txtT3.Text = (CommonDialog1.FileName)

ErrHandler:
   If Not Err = cdlCancel Then Resume Next

    Antennentypenliste_laden
    
End Sub
      
Private Sub Antennentypenliste_laden()
      
   'Mit Pfad aus txtT3.Text
   On Error Resume Next
   
   'Flexgrid auf 4 Kolonnen setzen, spez für Files mit CR und LF Marke
    flexT1.Clear
    flexT1.Cols = 4
    flexT1.Rows = 3

  ' FlexA1 mit File von Disk laden
    TextBoxT1.Text = ""   ' Textbox leeren
    
    Dim Text1
    Open txtT3.Text For Input As #1   ' Datei zum Einlesen öffnen.
    
    Do While Not EOF(1)   ' Schleife bis Dateiende.
        Input #1, Text1 ' Daten in zwei Variablen einlesen.
        TextBoxT1.Text = TextBoxT1.Text + Text1 + vbCrLf
    Loop

    Close #1   ' Datei schließen
   
  'Tabelle löschen
        flexT1.Clear
             
  'Ausfiltern der Kolonnenanzahl
        Dim längezeilenT As String
    
  'ermittelt die die ersten 4 Zeichen des Datensatzes
        längezeilenT = Left(TextBoxT1.Text, 4)
  'nimmt die letzen 2 Zeichen
    längezeilenT = Right(längezeilenT, 2)
    
    
  'schreibt den markierten String als Zeilenanzahl in die
  'MSFlexGrid Eigenschaft Rows!
    flexT1.Rows = längezeilenT + 2
   
  'Sperrt das Neuzeichnen des FlexTabelle.
    flexT1.Redraw = False
    
  ' ermittelt die Anzahlt der Zeilen und Spalten.
  ' ermittelt die Strings, welche zwischen den doppelpunkten(:)liegen,
  ' und schreibt die Strings in jede einzelne Zelle.
    Dim XT As Long           ' Variable für die Spalten
                              
    Dim StartT As Long       'Variable für den Markierungsanfang
    Dim EndeT As Long        'Variable für das Markierungsende
    EndeT = 1
    
    For YT = 0 To flexT1.Rows - 1                              'ermittelt die Anzahlt der Zeilen
        
        For XT = 0 To flexT1.Cols - 1                          'ermittelt die Anzahlt der Spalten
            
                    
            StartT = InStr(EndeT, TextBoxT1.Text, ":", 0)      'Sucht nach dem Simikolon und gibt die Position in einer Zahl aus,
            TextBoxT1.SelStart = StartT                        'Markierungsanfang setzen
            EndeT = InStr(StartT + 1, TextBoxT1.Text, ":", 0)  'Markierungsende errechnen
            TextBoxT1.SelLength = EndeT - 1 - StartT           'Markierungsende setzen.
        
                    
           'ändert die aktuellen Zelle entsprechend der For-Next-Umläufe
            flexT1.Col = XT
            flexT1.Row = YT

           'schreibt den selektierten Text in die aktive Zelle
           
            flexT1.Text = TextBoxT1.SelText
            
           'ändert die aktuellen Zelle wieder entsprechend der For-Next-Umläufe,
           'um den Zustand vor der farbeänderung herzustellen:
            flexT1.Col = XT
        
        Next XT
    
    Next YT

ErrHandler:
   If Not Err = cdlCancel Then Resume Next
            
     'Flexgrid auf 3 Kolonnen zurücksetzen
           flexT1.Cols = 3
    
    'Aktiviert das Neuzeichnen des FlexTabelle.
           flexT1.Redraw = True
 
End Sub


 Sub Antennentypenliste_speichern()

'Typen Datei FlexT1 auf Disk Speichern
    
    On Error Resume Next

  ' Sicherungspfad definieren
    txtT3.Text = App.Path & "\" & "Ant_Dat" & "\" & (flexT1.TextMatrix(1, 1)) & "_" & (flexT1.TextMatrix(1, 2)) & ".typ"
    StatusBar1.Panels(1) = txtT3.Text 'App.Path & "\Ant_Dat" & "\" & flexT1.TextMatrix(1, 1) & ".ant"
    
  ' löscht den Inhalt der Richtextbox
    TextBoxT1.Text = ""   'Textfeld löschen
    flexT1.TextMatrix(0, 1) = YT - 2  'Zeilenanzahl in Flexgrid eintragen
    
  ' Sperrt das Neuzeichnen des FlexTabelle.
    flexT1.Redraw = False
    
  ' ermittelt die Anzahlt der Zeilen und Spalten und schreibt den Inhalt jeder Zelle
  ' von einem Doppelpunkt (:) getrennt, in die Textbox
  
   txtT1.Text = ""
   
   Dim YTa As String
   Dim ZT As Long
   
   For ZT = 0 To 1   'Zwei Durchläufe damit Zeilenzahl richtig übernommen wird
   
   TextBoxT1.Text = ""   ' Textbox leeren
   flexT1.TextMatrix(0, 1) = YTa
    For YT = 0 To flexT1.Rows - 1
        Dim XT As Long
        For XT = 0 To flexT1.Cols - 1
            flexT1.Col = XT
            flexT1.Row = YT
            TextBoxT1.Text = TextBoxT1.Text + ":" + flexT1.Text
        Next XT
            'Zeilenumbruch
            TextBoxT1.Text = TextBoxT1.Text + ":" + Chr(13) + Chr(10)
        
    Next YT
    
  ' schreibt als letztes Zeichen in die RichTextbox ein Pipe-Zeichen(|),
  ' ansonsten würde der Inhalt der letzten Zelle nicht ausgelesen werden,
  ' da der Inhalt nicht zwischen zwei Pipe-Zeichen stehen würde.
    TextBoxT1.Text = TextBoxT1.Text + ":"
    
  ' Aktiviert das Neuzeichnen des FlexTabelle.
    flexT1.Redraw = True
    
    
    txtT1.Text = YT - 2
    
    'Damit bei Typenanzahl kleiner 10 ein 0 vorangestellt wird
    'Sonst erkennt die Laderutine die Anzahl Typen falsch.
    If txtT1.Text < 10 Then txtT1.Text = txtT1.Text / 100
      
    ' legt die letzen zwei Zeichen fest (ganze Zahl)
    YTa = Right(txtT1.Text, 2)
    
    Next ZT
  ' Anzeige des Pfads wo die Datei gespeichert wird
    
    Open txtT3.Text For Output As #1   ' Datei zur Ausgabe öffnen.
    Print #1, TextBoxT1.Text   ' Text in Datei schreiben.
    Close #1   ' Datei schließen.
  
    
ErrHandler:
   If Not Err = cdlCancel Then Resume Next
   
End Sub

Private Sub Form_Unload(Cancel As Integer)
    
    If Me.WindowState <> vbMinimized Then
        SaveSetting App.Title, "Settings", "TypLeft", Me.Left
        SaveSetting App.Title, "Settings", "TypTop", Me.Top
        SaveSetting App.Title, "Settings", "TypWidth", Me.Width
        SaveSetting App.Title, "Settings", "TypHeight", Me.Height
    End If

End Sub


Private Sub mnu_Beenden_Click()

    Unload Me

End Sub

Private Sub mnu_Deltyp_Click()

    Antennentypen_loschen

End Sub

'==================Ende=====================

'Private Sub mnu_Typeload_Click()

'    Antennentypenliste_laden

'End Sub

'Private Sub mnu_Typesave_Click()

'    Antennentypenliste_speichern

'End Sub

Private Sub mnuAdtype_Click()   'nicht mehr vorhanden

    AddNewType

End Sub

 
 Sub Antennentyp_ubertragen()
 
 'Wenn neue Antennnendaten eingegeben werden
 AddNewType
 
 flexT1.TextMatrix(flexT1.Rows - 1, 2) = frmAntEingabe.FlexA1.TextMatrix(1, 2)
 
 Antennentypenliste_speichern     'Typenliste speichern


 End Sub



Private Sub AddNewType()

    'Flex um eine Reihe erhöhen für neuen Typ
    
    'FlexT1 um eine Reihe erhöhen
    flexT1.Rows = flexT1.Rows + 1
    'FlexT1 Typenzahl um 1 erhöhen
    flexT1.TextMatrix(flexT1.Rows - 1, 1) = 1 + flexT1.TextMatrix(flexT1.Rows - 2, 1)
    
    'Anzahl der Typen in Reihe 0 um 1 erhöhen.
    flexT1.TextMatrix(0, 1) = 1 + flexT1.TextMatrix(flexT1.Rows - 2, 1)
    
    'Cursor in erste Kolonne des letzten Eintrages setzen.
    flexT1.Col = 1
    flexT1.Row = flexT1.Rows - 1
    
    

End Sub


Private Sub cmdT4_Click()

    Antennentypen_loschen

End Sub


Private Sub Antennentypen_loschen()

Dim setPath As String
 

    On Error GoTo FindeDatei_Err
    
    'Antennentyp Datei löschen
    setPath = frmAntEingabe.StatusBar1.Panels(1)
    txtT2.Text = flexT1.TextMatrix(flexT1.Row, 2) & ".ant"

    Kill txtT2.Text   'Datei löschen

    frmAntEingabe.FlexGridA1_löschen   'Sub FelxA1 löschen und mit Muster füllen

    
    'Einen Eintrag in der Liste löschen und Liste neu formatieren
        
    flexT1.TextMatrix(flexT1.Row, 2) = ""  'löscht den Antennentyp
    
    'Wenn nur ein Typ vorhanden Sub verlassen
    If flexT1.Rows = 3 Then
        Exit Sub
    End If
            
    Dim x As Integer
    Dim y As Integer

    For x = 2 To flexT1.Rows - 1
        
        If flexT1.TextMatrix(x, 2) = Ascii Then  'Sucht nach dem gelöscheten Antennentyp
    
            y = x
        
            'Wenn letzter Eintrag in Flexgrid
            For y = y To flexT1.Rows - 1
                If y = flexT1.Rows - 1 Then
                    flexT1.TextMatrix(y, 1) = ""
                    flexT1.TextMatrix(y, 2) = ""
                Else
        
                   'verschiebt den folgenden Eintrag in die aktuelle Reihe
                    flexT1.TextMatrix(y, 2) = flexT1.TextMatrix(y + 1, 2)
                End If
        
                'wenn letze Reihe erreicht, Inhalt löschen, Flex.Rows -1 und Sub verlassen
                If y = flexT1.Rows - 1 Then
                    flexT1.TextMatrix(y, 1) = ""
                    flexT1.TextMatrix(y, 2) = ""
                    flexT1.Rows = flexT1.Rows - 1
                    
                    'Wenn nur noch ein Eintrag vorhanden ist Sub verlassen
                    If flexT1.Rows = 3 Then
                        flexT1.TextMatrix(0, 1) = 1
                        Antennentypenliste_speichern
                        Exit Sub
                    End If
                    
                    'Anzahl der Typen in Reihe 0 um 1 erhöhen.
                    flexT1.TextMatrix(0, 1) = 1 + flexT1.TextMatrix(flexT1.Rows - 2, 1)
                    Antennentypenliste_speichern
                Exit Sub
        
                End If
            Next y
        End If
    Next x

    'Anzahl der Typen in Reihe 0 um 1 erhöhen.
    flexT1.TextMatrix(0, 1) = 1 + flexT1.TextMatrix(flexT1.Rows - 2, 1)

Antennentypenliste_speichern


Exit Sub

'Fehlerbehandlung
FindeDatei_Err:

    MsgBox "Datei nicht vorhanden", vbExclamation, "Error"


End Sub


