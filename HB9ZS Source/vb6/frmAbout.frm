VERSION 5.00
Begin VB.Form frmAbout 
   BorderStyle     =   3  'Fester Dialog
   Caption         =   "Info "
   ClientHeight    =   3840
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5865
   ClipControls    =   0   'False
   Icon            =   "frmAbout.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3840
   ScaleWidth      =   5865
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'Fenstermitte
   Tag             =   "Info Projekt1"
   Begin VB.PictureBox picIcon 
      AutoSize        =   -1  'True
      BackColor       =   &H00C0C0C0&
      BorderStyle     =   0  'Kein
      ClipControls    =   0   'False
      Height          =   480
      Left            =   480
      Picture         =   "frmAbout.frx":0442
      ScaleHeight     =   480
      ScaleMode       =   0  'Benutzerdefiniert
      ScaleWidth      =   480
      TabIndex        =   2
      TabStop         =   0   'False
      Top             =   360
      Width           =   480
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Height          =   345
      Left            =   4245
      MouseIcon       =   "frmAbout.frx":0884
      MousePointer    =   99  'Benutzerdefiniert
      TabIndex        =   0
      Tag             =   "OK"
      Top             =   2625
      Width           =   1350
   End
   Begin VB.CommandButton cmdSysInfo 
      Caption         =   "&System-Info..."
      Height          =   345
      Left            =   4260
      MouseIcon       =   "frmAbout.frx":0B8E
      MousePointer    =   99  'Benutzerdefiniert
      TabIndex        =   1
      Tag             =   "&System-Info..."
      Top             =   3240
      Width           =   1335
   End
   Begin VB.Label lblDescription 
      Caption         =   "Immissionsberechnung für Amateurfunkstationen in der Schweiz"
      ForeColor       =   &H00000000&
      Height          =   810
      Left            =   1800
      TabIndex        =   6
      Tag             =   "Anwendungsbeschreibung"
      Top             =   1320
      Width           =   3375
   End
   Begin VB.Label lblTitle 
      Caption         =   "Feldstärkeberechnung"
      ForeColor       =   &H00000000&
      Height          =   480
      Left            =   1800
      TabIndex        =   5
      Tag             =   "Anwendungstitel"
      Top             =   240
      Width           =   3375
   End
   Begin VB.Line Line1 
      BorderColor     =   &H00808080&
      BorderStyle     =   6  'Innen ausgefüllt
      Index           =   1
      X1              =   225
      X2              =   5657
      Y1              =   2430
      Y2              =   2430
   End
   Begin VB.Line Line1 
      BorderColor     =   &H00FFFFFF&
      BorderWidth     =   2
      Index           =   0
      X1              =   240
      X2              =   5657
      Y1              =   2445
      Y2              =   2445
   End
   Begin VB.Label lblVersion 
      Caption         =   "Version"
      Height          =   225
      Left            =   1770
      TabIndex        =   4
      Tag             =   "Version"
      Top             =   780
      Width           =   3375
   End
   Begin VB.Label lblDisclaimer 
      Caption         =   $"frmAbout.frx":0E98
      ForeColor       =   &H00000000&
      Height          =   1065
      Left            =   255
      TabIndex        =   3
      Tag             =   "Warnung: ..."
      Top             =   2625
      Width           =   3870
   End
End
Attribute VB_Name = "frmAbout"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'===============================================
'Fenster für allgemeine Programminfos
'Deutsch
' 28. 09. 2003 hb9zs
'===============================================

Option Explicit

' Registrierungsschlüssel - Sicherheitsoptionen...
Const KEY_ALL_ACCESS = &H2003F
                                          

' Registrierungsschlüssel - Sicherheitsoptionen...
Const HKEY_LOCAL_MACHINE = &H80000002
Const ERROR_SUCCESS = 0
Const REG_SZ = 1                         ' Null-terminierte Unicode-Zeichenfolge
Const REG_DWORD = 4                      ' 32-Bit-Zahl


Const gREGKEYSYSINFOLOC = "SOFTWARE\Microsoft\Shared Tools Location"
Const gREGVALSYSINFOLOC = "MSINFO"
Const gREGKEYSYSINFO = "SOFTWARE\Microsoft\Shared Tools\MSINFO"
Const gREGVALSYSINFO = "PATH"


Private Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey As Long, ByVal lpSubKey As String, ByVal ulOptions As Long, ByVal samDesired As Long, ByRef phkResult As Long) As Long
Private Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Long, ByVal lpValueName As String, ByVal lpReserved As Long, ByRef lpType As Long, ByVal lpData As String, ByRef lpcbData As Long) As Long
Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Long) As Long

Private Sub Form_Load()
    
   lblVersion.Caption = LoadResString(1451 + RS) & " " & App.Major & "." & App.Minor & "." & App.Revision
        
   frmAbout.Caption = LoadResString(1454 + RS)
   lblTitle.Caption = LoadResString(1450 + RS)
   lblDescription.Caption = LoadResString(1452 + RS)
   lblDisclaimer.Caption = LoadResString(1453 + RS)
    
End Sub

Private Sub cmdSysInfo_Click()

        Call StartSysInfo
        
End Sub


Private Sub cmdOK_Click()

        Unload Me
        
End Sub


Public Sub StartSysInfo()

    On Error GoTo SysInfoErr


        Dim rc As Long
        Dim SysInfoPath As String
        

        ' Versuchen Namen und Pfad des Systeminfo-Programms aus der Registrierung zu lesen...
        If GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFO, gREGVALSYSINFO, SysInfoPath) Then
        ' Versuchen nur den Pfad des Systeminfo-Programms aus der Registrierung zu lesen...
        ElseIf GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFOLOC, gREGVALSYSINFOLOC, SysInfoPath) Then
                ' Sicherstellen, daß es sich um bekannte 32-Bit-Version handelt.
                If (Dir(SysInfoPath & "\MSINFO32.EXE") <> "") Then
                        SysInfoPath = SysInfoPath & "\MSINFO32.EXE"
                Else
                        GoTo SysInfoErr
                End If
        
        Else
                GoTo SysInfoErr
        End If
        

        Call Shell(SysInfoPath, vbNormalFocus)
        

        Exit Sub
SysInfoErr:
        MsgBox "Systeminformationsprogramm zur Zeit nicht verfügbar.", vbOKOnly

End Sub


Public Function GetKeyValue(KeyRoot As Long, KeyName As String, SubKeyRef As String, ByRef KeyVal As String) As Boolean
        
        Dim i As Long               ' Schleifenzähler
        Dim rc As Long              ' Rückgabe-Code
        Dim hKey As Long            ' Handle für geöffneten Registrierungsschlüssel
        Dim hDepth As Long          '
        Dim KeyValType As Long      ' Datentyp eines Registrierungsschlüssels
        Dim tmpVal As String        ' Teporärer Speicher für einen Registrierungswert
        Dim KeyValSize As Long      ' Größe der Registrierungsschlüssel-Variablen
        '------------------------------------------------------------
        ' Registrierungsschlüssel unter Stammverzeichnis
        ' öffnen {HKEY_LOCAL_MACHINE...}
        '------------------------------------------------------------
        rc = RegOpenKeyEx(KeyRoot, KeyName, 0, KEY_ALL_ACCESS, hKey) ' Open Registry Key
        

        If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError          ' Fehlerbehandlung...
        

        tmpVal = String$(1024, 0)                               ' Speicher für Variable reservieren
        KeyValSize = 1024                                       ' Größe der Variablen speichern
        

        '------------------------------------------------------------
        ' Registrierungswert abrufen...
        '------------------------------------------------------------
        rc = RegQueryValueEx(hKey, SubKeyRef, 0, KeyValType, tmpVal, KeyValSize)    ' Get/Create Key Value
                                                

        If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError          ' Fehlerbehandlung
        

        tmpVal = VBA.Left(tmpVal, InStr(tmpVal, VBA.Chr(0)) - 1)
        '------------------------------------------------------------
        ' Bestimmen des Datentyps für die Konvertierung...
        '------------------------------------------------------------
        Select Case KeyValType                                  ' Durchsuchen der Datentypen...
        Case REG_SZ                                             ' Datentyp Zeichenfolge
                KeyVal = tmpVal                                     ' Kopieren der Zeichenfolge
        Case REG_DWORD                                          ' Datentyp Doppelwort
                For i = Len(tmpVal) To 1 Step -1                    ' Konvertieren der einzelnen Bits
                        KeyVal = KeyVal + Hex(Asc(Mid(tmpVal, i, 1)))   ' Wert Zeichen für Zeichen erstellen
                Next
                KeyVal = Format$("&h" + KeyVal)                     ' Doppelwort in Zeichenfoge umwandeln
        End Select
        

        GetKeyValue = True                                      ' Wert für Erfolg zurückgeben
        rc = RegCloseKey(hKey)                                  ' Registrierungsschlüssel schließen
        Exit Function                                           ' Funktion verlassen
        

GetKeyError:    ' Aufräumen, nachdem ein Fehler aufgetreten ist...
        KeyVal = ""                                             ' Rückgabewert auf leere Zeichenfolge setzen
        GetKeyValue = False                                     ' Wert für Fehler zurückliefern
        rc = RegCloseKey(hKey)                                  ' Registrierungsschlüssel schließen
End Function


