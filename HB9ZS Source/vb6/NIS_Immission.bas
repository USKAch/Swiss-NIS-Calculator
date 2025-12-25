Attribute VB_Name = "Module1"
'==========================================
'modul Allgemeine Deklarationen
' 11.12.2001  hb9zs
'==========================================

Public RS As Long   'Def für Sprachauswahl
                    'RS = 0 Deutsch
                    'RS = 1000 Französich
                    'RS = 2000 Italienisch

Public blk As Integer 'Blinker starten = 1

Public cboA1_sperren As Integer

Public C1 As Integer

Public Herst As String

Public Ty As String

Public No_Diagram As Integer

'FlexGrid Edit definitionen
Public clsFGEdit As clsFlexGridEdit
Public clsFGEdit2 As clsFlexGridEdit2
