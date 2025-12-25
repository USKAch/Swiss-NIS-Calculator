VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.2#0"; "MSCOMCTL.OCX"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "TABCTL32.OCX"
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form frmMain 
   BorderStyle     =   0  'Kein
   Caption         =   "Blatt"
   ClientHeight    =   9975
   ClientLeft      =   4635
   ClientTop       =   2505
   ClientWidth     =   10575
   Icon            =   "frmMain.frx":0000
   KeyPreview      =   -1  'True
   LinkTopic       =   "MDIForm1"
   MDIChild        =   -1  'True
   ScaleHeight     =   9975
   ScaleWidth      =   10575
   ShowInTaskbar   =   0   'False
   Visible         =   0   'False
   Begin TabDlg.SSTab Immissionsberechnung 
      Height          =   9735
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   10335
      _ExtentX        =   18230
      _ExtentY        =   17171
      _Version        =   393216
      TabHeight       =   520
      BackColor       =   -2147483638
      TabCaption(0)   =   "1201"
      TabPicture(0)   =   "frmMain.frx":0442
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "frmDatenfeld"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "1401"
      TabPicture(1)   =   "frmMain.frx":045E
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "lblK2"
      Tab(1).Control(1)=   "frmK1"
      Tab(1).Control(2)=   "FlexKabel1"
      Tab(1).Control(3)=   "cmdK2"
      Tab(1).Control(4)=   "TextBoxK1"
      Tab(1).ControlCount=   5
      TabCaption(2)   =   "1501"
      TabPicture(2)   =   "frmMain.frx":047A
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "frmA1"
      Tab(2).Control(0).Enabled=   0   'False
      Tab(2).ControlCount=   1
      Begin VB.TextBox TextBoxK1 
         Height          =   285
         Left            =   -66840
         TabIndex        =   93
         Top             =   600
         Visible         =   0   'False
         Width           =   1455
      End
      Begin VB.CommandButton cmdK2 
         Caption         =   "Zusätzlich eigene Kabeldatendaten von der Disk laden"
         Height          =   855
         Left            =   -71280
         MouseIcon       =   "frmMain.frx":0496
         MousePointer    =   99  'Benutzerdefiniert
         TabIndex        =   92
         Top             =   480
         Width           =   2652
      End
      Begin MSFlexGridLib.MSFlexGrid FlexKabel1 
         Height          =   855
         Left            =   -74640
         TabIndex        =   91
         Top             =   480
         Visible         =   0   'False
         Width           =   3375
         _ExtentX        =   5953
         _ExtentY        =   1508
         _Version        =   393216
      End
      Begin VB.Frame frmA1 
         BorderStyle     =   0  'Kein
         Height          =   9015
         Left            =   -74880
         TabIndex        =   38
         Top             =   480
         Width           =   9975
         Begin VB.TextBox txtA19 
            Height          =   285
            Left            =   6480
            MouseIcon       =   "frmMain.frx":07A0
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   90
            ToolTipText     =   "manual entry of antenna typ"
            Top             =   3120
            Width           =   2292
         End
         Begin VB.TextBox txtA11 
            Alignment       =   1  'Rechts
            Height          =   285
            Left            =   7800
            MouseIcon       =   "frmMain.frx":0AAA
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   83
            Text            =   "0"
            Top             =   3840
            Width           =   375
         End
         Begin VB.TextBox txtA10 
            Alignment       =   1  'Rechts
            Height          =   285
            Left            =   7800
            MouseIcon       =   "frmMain.frx":0DB4
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   81
            Text            =   "0"
            Top             =   3480
            Width           =   375
         End
         Begin VB.TextBox txtA8 
            Height          =   285
            Left            =   6480
            MouseIcon       =   "frmMain.frx":10BE
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   78
            Text            =   "txtA8"
            Top             =   3480
            Width           =   495
         End
         Begin VB.TextBox txtA9 
            Height          =   285
            Left            =   6480
            MouseIcon       =   "frmMain.frx":13C8
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   77
            Text            =   "txtA9"
            Top             =   3840
            Width           =   495
         End
         Begin VB.CommandButton cmdA1 
            Caption         =   "Strahlungsdiagramm"
            Height          =   495
            Left            =   7560
            MouseIcon       =   "frmMain.frx":16D2
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   72
            Top             =   1200
            Width           =   1695
         End
         Begin VB.TextBox TextBox1 
            Height          =   285
            Left            =   5880
            TabIndex        =   70
            Text            =   "TextBox1"
            Top             =   2400
            Visible         =   0   'False
            Width           =   1455
         End
         Begin VB.TextBox TextBoxH1 
            Height          =   285
            Left            =   7320
            TabIndex        =   69
            Text            =   "TextBoxH1"
            Top             =   1560
            Visible         =   0   'False
            Width           =   2535
         End
         Begin VB.TextBox TextBoxT1 
            Height          =   285
            Left            =   7320
            ScrollBars      =   1  'Horizontal
            TabIndex        =   68
            Top             =   1200
            Visible         =   0   'False
            Width           =   2535
         End
         Begin VB.Frame Frame3 
            Caption         =   "1507"
            Height          =   735
            Left            =   6240
            TabIndex        =   53
            Top             =   240
            Width           =   2655
            Begin VB.TextBox txtA7 
               Alignment       =   1  'Rechts
               BackColor       =   &H00C0FFFF&
               Enabled         =   0   'False
               Height          =   285
               Left            =   1440
               Locked          =   -1  'True
               TabIndex        =   55
               TabStop         =   0   'False
               ToolTipText     =   "Gewählter Winkel aus Tabelle unten"
               Top             =   240
               Width           =   495
            End
            Begin VB.TextBox txtA6 
               Alignment       =   1  'Rechts
               BackColor       =   &H00C0FFFF&
               Enabled         =   0   'False
               Height          =   285
               Left            =   120
               Locked          =   -1  'True
               TabIndex        =   54
               TabStop         =   0   'False
               Text            =   "0"
               Top             =   240
               Width           =   615
            End
            Begin VB.Label lblA11 
               Caption         =   "Grad"
               Height          =   255
               Left            =   2040
               TabIndex        =   57
               Top             =   300
               Width           =   375
            End
            Begin VB.Label lblA10 
               Caption         =   "dB  bei"
               Height          =   255
               Left            =   840
               TabIndex        =   56
               Top             =   300
               Width           =   1095
            End
         End
         Begin VB.TextBox txtA3 
            Height          =   255
            Left            =   3960
            TabIndex        =   52
            TabStop         =   0   'False
            Text            =   "txtA3"
            Top             =   1800
            Visible         =   0   'False
            Width           =   3015
         End
         Begin VB.Frame frmA6 
            Caption         =   " 1510    "
            Height          =   4650
            Left            =   120
            TabIndex        =   50
            Top             =   4320
            Width           =   9735
            Begin MSComctlLib.ProgressBar ProgressBar1 
               Height          =   255
               Left            =   3600
               TabIndex        =   71
               Top             =   1680
               Width           =   2895
               _ExtentX        =   5106
               _ExtentY        =   450
               _Version        =   393216
               Appearance      =   1
               Max             =   250
            End
            Begin MSFlexGridLib.MSFlexGrid flexA1 
               Height          =   4195
               Left            =   120
               TabIndex        =   51
               Top             =   240
               Width           =   9480
               _ExtentX        =   16722
               _ExtentY        =   7408
               _Version        =   393216
               Rows            =   16
               Cols            =   13
               BackColorSel    =   -2147483646
               AllowBigSelection=   0   'False
               ScrollBars      =   0
               MousePointer    =   99
               MouseIcon       =   "frmMain.frx":19DC
            End
         End
         Begin VB.Frame frmA3 
            Caption         =   "1503"
            Height          =   2895
            Left            =   120
            TabIndex        =   49
            Top             =   1200
            Width           =   1575
            Begin MSFlexGridLib.MSFlexGrid flexH1 
               Height          =   2535
               Left            =   120
               TabIndex        =   41
               Top             =   240
               Width           =   1350
               _ExtentX        =   2381
               _ExtentY        =   4471
               _Version        =   393216
               AllowBigSelection=   0   'False
               ScrollTrack     =   -1  'True
               MousePointer    =   99
               MouseIcon       =   "frmMain.frx":1CF6
            End
         End
         Begin VB.Frame frmA5 
            Caption         =   "1506"
            Height          =   735
            Left            =   4080
            TabIndex        =   46
            Top             =   240
            Width           =   1815
            Begin VB.TextBox txtA2 
               Alignment       =   1  'Rechts
               BackColor       =   &H00C0FFFF&
               Enabled         =   0   'False
               Height          =   285
               Left            =   120
               Locked          =   -1  'True
               TabIndex        =   47
               TabStop         =   0   'False
               Text            =   "0"
               Top             =   240
               Width           =   615
            End
            Begin VB.Label lblA6 
               Caption         =   "dBi"
               Height          =   210
               Left            =   840
               TabIndex        =   48
               Top             =   300
               Width           =   255
            End
         End
         Begin VB.Frame frm4 
            Caption         =   "1504"
            Height          =   3975
            Left            =   1920
            TabIndex        =   44
            Top             =   240
            Width           =   1815
            Begin MSFlexGridLib.MSFlexGrid flexT1 
               Height          =   3255
               Left            =   120
               TabIndex        =   42
               Top             =   600
               Width           =   1545
               _ExtentX        =   2725
               _ExtentY        =   5741
               _Version        =   393216
               AllowBigSelection=   0   'False
               ScrollTrack     =   -1  'True
               ScrollBars      =   2
               MousePointer    =   99
               MouseIcon       =   "frmMain.frx":2010
            End
            Begin VB.Label lblA9 
               Caption         =   "1505"
               Height          =   255
               Left            =   120
               TabIndex        =   45
               Top             =   270
               Width           =   1335
            End
         End
         Begin VB.Frame frmA2 
            Caption         =   "1502"
            Height          =   735
            Left            =   240
            TabIndex        =   39
            Top             =   240
            Width           =   1455
            Begin VB.ComboBox cboA1 
               Height          =   315
               ItemData        =   "frmMain.frx":232A
               Left            =   480
               List            =   "frmMain.frx":232C
               MouseIcon       =   "frmMain.frx":232E
               MousePointer    =   99  'Benutzerdefiniert
               Style           =   2  'Dropdown-Liste
               TabIndex        =   40
               ToolTipText     =   "Frequenzwahl"
               Top             =   240
               Width           =   855
            End
            Begin VB.Label lblA8 
               Caption         =   "MHz"
               Height          =   255
               Left            =   85
               TabIndex        =   43
               Top             =   300
               Width           =   375
            End
         End
         Begin VB.Label Label4 
            Caption         =   "Label4"
            Height          =   255
            Left            =   8880
            TabIndex        =   117
            Top             =   360
            Visible         =   0   'False
            Width           =   375
         End
         Begin VB.Label lblA19 
            Alignment       =   1  'Rechts
            Caption         =   "Herst, Typ"
            Height          =   255
            Left            =   4440
            TabIndex        =   89
            Top             =   3165
            Width           =   1935
         End
         Begin VB.Label lblA26 
            Caption         =   "1755"
            Height          =   255
            Left            =   8280
            TabIndex        =   84
            Top             =   3890
            Width           =   615
         End
         Begin VB.Label lblA25 
            Caption         =   "1755"
            Height          =   255
            Left            =   8280
            TabIndex        =   82
            Top             =   3530
            Width           =   615
         End
         Begin VB.Label lblA24 
            Alignment       =   1  'Rechts
            Caption         =   "1754"
            Height          =   255
            Left            =   7080
            TabIndex        =   80
            Top             =   3890
            Width           =   615
         End
         Begin VB.Label lblA23 
            Alignment       =   1  'Rechts
            Caption         =   "1754"
            Height          =   255
            Left            =   7080
            TabIndex        =   79
            Top             =   3530
            Width           =   615
         End
         Begin VB.Label lblA22 
            Alignment       =   1  'Rechts
            Caption         =   "1753"
            Height          =   255
            Left            =   3960
            TabIndex        =   76
            Top             =   3890
            Width           =   2415
         End
         Begin VB.Label lblA21 
            Alignment       =   1  'Rechts
            Caption         =   "1752"
            Height          =   255
            Left            =   3960
            TabIndex        =   75
            Top             =   3530
            Width           =   2415
         End
         Begin VB.Label lblA18 
            ForeColor       =   &H000000FF&
            Height          =   855
            Left            =   7560
            TabIndex        =   74
            Top             =   1800
            Width           =   1695
            WordWrap        =   -1  'True
         End
         Begin VB.Label lblA17 
            Height          =   495
            Left            =   7560
            TabIndex        =   73
            Top             =   2280
            Width           =   1695
            WordWrap        =   -1  'True
         End
         Begin VB.Label lblT1 
            Caption         =   "lblT1"
            Height          =   255
            Left            =   6480
            TabIndex        =   67
            Top             =   1320
            Visible         =   0   'False
            Width           =   3135
         End
         Begin VB.Label lblH1 
            Caption         =   "lblH1"
            Height          =   255
            Left            =   6480
            TabIndex        =   66
            Top             =   1080
            Visible         =   0   'False
            Width           =   3135
         End
         Begin VB.Label lblh14 
            Caption         =   "lblH14"
            Height          =   255
            Left            =   9240
            TabIndex        =   65
            Top             =   2640
            Visible         =   0   'False
            Width           =   495
         End
         Begin VB.Label lblH13 
            Caption         =   "lblH13"
            Height          =   255
            Left            =   9240
            TabIndex        =   64
            Top             =   2400
            Visible         =   0   'False
            Width           =   495
         End
         Begin VB.Label lblA20 
            Caption         =   "1509"
            Height          =   855
            Left            =   3960
            TabIndex        =   63
            Top             =   2040
            Width           =   2895
         End
         Begin VB.Label lblA16 
            Caption         =   "0"
            Height          =   255
            Left            =   8880
            TabIndex        =   62
            Top             =   2640
            Visible         =   0   'False
            Width           =   255
         End
         Begin VB.Label lblA15 
            Caption         =   "0"
            Height          =   255
            Left            =   8880
            TabIndex        =   61
            Top             =   2400
            Visible         =   0   'False
            Width           =   255
         End
         Begin VB.Label lblA2 
            Caption         =   "lblA2"
            Height          =   255
            Left            =   5400
            TabIndex        =   60
            Top             =   2400
            Visible         =   0   'False
            Width           =   735
         End
         Begin VB.Label lblA7 
            Caption         =   "lblA7"
            Height          =   255
            Left            =   5400
            TabIndex        =   59
            Top             =   2640
            Visible         =   0   'False
            Width           =   495
         End
         Begin VB.Label Label1 
            Caption         =   "1508"
            Height          =   735
            Left            =   3960
            TabIndex        =   58
            Top             =   1200
            Visible         =   0   'False
            Width           =   2175
         End
      End
      Begin VB.Frame frmDatenfeld 
         BorderStyle     =   0  'Kein
         Height          =   9135
         Left            =   240
         TabIndex        =   17
         Top             =   360
         Width           =   9855
         Begin VB.ComboBox cboG1 
            Height          =   315
            ItemData        =   "frmMain.frx":2638
            Left            =   6380
            List            =   "frmMain.frx":263A
            Style           =   2  'Dropdown-Liste
            TabIndex        =   124
            Top             =   6340
            Width           =   2700
         End
         Begin VB.Timer Timer2 
            Enabled         =   0   'False
            Interval        =   20
            Left            =   8880
            Top             =   4080
         End
         Begin VB.ComboBox cboM1 
            Height          =   288
            ItemData        =   "frmMain.frx":263C
            Left            =   5450
            List            =   "frmMain.frx":263E
            MouseIcon       =   "frmMain.frx":2640
            Style           =   2  'Dropdown-Liste
            TabIndex        =   109
            Top             =   2280
            Width           =   900
         End
         Begin VB.ComboBox cboF1 
            BeginProperty DataFormat 
               Type            =   0
               Format          =   "0"
               HaveTrueFalseNull=   0
               FirstDayOfWeek  =   0
               FirstWeekOfYear =   0
               LCID            =   2055
               SubFormatType   =   0
            EndProperty
            Height          =   288
            ItemData        =   "frmMain.frx":2A82
            Left            =   5450
            List            =   "frmMain.frx":2A84
            MouseIcon       =   "frmMain.frx":2A86
            Style           =   2  'Dropdown-Liste
            TabIndex        =   108
            Top             =   1440
            Width           =   900
         End
         Begin VB.CommandButton cmd1 
            BackColor       =   &H0000FFFF&
            Caption         =   "Kolonne löschen"
            Height          =   495
            Left            =   7440
            MaskColor       =   &H8000000F&
            MouseIcon       =   "frmMain.frx":2D90
            MousePointer    =   99  'Benutzerdefiniert
            Style           =   1  'Grafisch
            TabIndex        =   107
            Top             =   5520
            Width           =   1215
         End
         Begin VB.Frame Frame4 
            BorderStyle     =   0  'Kein
            Height          =   1212
            Left            =   7080
            TabIndex        =   102
            Top             =   240
            Width           =   2292
            Begin VB.TextBox txt302 
               Height          =   285
               Left            =   0
               MouseIcon       =   "frmMain.frx":309A
               MousePointer    =   99  'Benutzerdefiniert
               TabIndex        =   106
               Top             =   840
               Width           =   2055
            End
            Begin VB.TextBox txt301 
               Height          =   285
               Left            =   0
               MouseIcon       =   "frmMain.frx":33A4
               MousePointer    =   99  'Benutzerdefiniert
               TabIndex        =   104
               Top             =   240
               Width           =   2055
            End
            Begin VB.Label lbl302 
               Caption         =   "1751"
               Height          =   252
               Left            =   0
               TabIndex        =   105
               Top             =   600
               Width           =   1692
            End
            Begin VB.Label lbl301 
               Caption         =   "1750"
               Height          =   252
               Left            =   0
               TabIndex        =   103
               Top             =   0
               Width           =   1452
            End
         End
         Begin VB.Frame Frame1 
            BorderStyle     =   0  'Kein
            Height          =   7695
            Left            =   600
            TabIndex        =   95
            Top             =   1200
            Width           =   5772
            Begin VB.TextBox Text1 
               Height          =   288
               Left            =   120
               TabIndex        =   98
               Text            =   "Text1"
               Top             =   6360
               Visible         =   0   'False
               Width           =   732
            End
            Begin MSFlexGridLib.MSFlexGrid FlexGrid2 
               Height          =   6132
               Left            =   4824
               TabIndex        =   96
               Top             =   240
               Width           =   936
               _ExtentX        =   1640
               _ExtentY        =   10821
               _Version        =   393216
               RowHeightMin    =   260
               HighLight       =   0
               ScrollBars      =   1
            End
            Begin MSFlexGridLib.MSFlexGrid FlexGrid1 
               Height          =   5856
               Left            =   240
               TabIndex        =   97
               Top             =   240
               Width           =   5652
               _ExtentX        =   9975
               _ExtentY        =   10319
               _Version        =   393216
               RowHeightMin    =   260
               AllowBigSelection=   0   'False
               ScrollBars      =   0
            End
            Begin VB.Label lbl06 
               Caption         =   "Kolonne verschieben ->      <-- F6       F7 -->"
               Height          =   495
               Left            =   3000
               TabIndex        =   110
               Top             =   6120
               Width           =   1695
               WordWrap        =   -1  'True
            End
            Begin VB.Label lblB1 
               BackColor       =   &H0080FFFF&
               BorderStyle     =   1  'Fest Einfach
               Caption         =   "lblB1"
               Height          =   252
               Left            =   4848
               TabIndex        =   99
               Top             =   0
               Width           =   876
            End
         End
         Begin VB.TextBox txt35 
            Height          =   285
            Left            =   9240
            MouseIcon       =   "frmMain.frx":36AE
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   88
            Top             =   2640
            Width           =   495
         End
         Begin VB.TextBox txt25 
            Height          =   285
            Left            =   9240
            MouseIcon       =   "frmMain.frx":39B8
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   86
            Top             =   2280
            Width           =   495
         End
         Begin VB.Timer Timer1 
            Left            =   8880
            Top             =   3600
         End
         Begin VB.TextBox txt05 
            Height          =   285
            Left            =   4320
            MouseIcon       =   "frmMain.frx":3CC2
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   20
            Top             =   480
            Width           =   1695
         End
         Begin VB.TextBox txt04 
            Height          =   285
            Left            =   2640
            MouseIcon       =   "frmMain.frx":3FCC
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   19
            Top             =   480
            Width           =   1695
         End
         Begin VB.TextBox txt03 
            Height          =   285
            Left            =   840
            MouseIcon       =   "frmMain.frx":42D6
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   18
            Top             =   480
            Width           =   1815
         End
         Begin VB.TextBox txt02 
            BeginProperty DataFormat 
               Type            =   0
               Format          =   "AA9AAA"
               HaveTrueFalseNull=   0
               FirstDayOfWeek  =   0
               FirstWeekOfYear =   0
               LCID            =   2055
               SubFormatType   =   0
            EndProperty
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   300
            Left            =   840
            MaxLength       =   6
            MouseIcon       =   "frmMain.frx":45E0
            MousePointer    =   99  'Benutzerdefiniert
            TabIndex        =   21
            Top             =   840
            Visible         =   0   'False
            Width           =   1095
         End
         Begin VB.CommandButton cmd03 
            BackColor       =   &H00C0C0C0&
            Caption         =   "1276"
            Height          =   375
            Left            =   7440
            MaskColor       =   &H00E0E0E0&
            MouseIcon       =   "frmMain.frx":48EA
            MousePointer    =   99  'Benutzerdefiniert
            Style           =   1  'Grafisch
            TabIndex        =   22
            Top             =   3600
            Width           =   1215
         End
         Begin VB.Label Label5 
            Caption         =   "©"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   12
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   8350
            TabIndex        =   125
            Top             =   8350
            Width           =   200
         End
         Begin VB.Label Label2 
            Caption         =   "Label2"
            Height          =   372
            Left            =   8640
            TabIndex        =   101
            Top             =   7440
            Visible         =   0   'False
            Width           =   492
         End
         Begin VB.Label Label3 
            Caption         =   "Label3"
            Height          =   132
            Left            =   7440
            TabIndex        =   100
            Top             =   8160
            Visible         =   0   'False
            Width           =   492
         End
         Begin VB.Line Line4 
            X1              =   6480
            X2              =   6600
            Y1              =   2880
            Y2              =   2880
         End
         Begin VB.Line Line3 
            X1              =   6480
            X2              =   6600
            Y1              =   2280
            Y2              =   2280
         End
         Begin VB.Line Line2 
            X1              =   6360
            X2              =   6480
            Y1              =   2580
            Y2              =   2580
         End
         Begin VB.Line Line1 
            X1              =   6480
            X2              =   6480
            Y1              =   2280
            Y2              =   2860
         End
         Begin VB.Label lbl34 
            Caption         =   "1724 "
            Height          =   255
            Left            =   6480
            TabIndex        =   87
            Top             =   2640
            Width           =   2775
         End
         Begin VB.Label lbl24 
            Caption         =   "1723"
            Height          =   255
            Left            =   6480
            TabIndex        =   85
            Top             =   2325
            Width           =   2775
         End
         Begin VB.Label lbl124 
            ForeColor       =   &H00FF0000&
            Height          =   192
            Left            =   6456
            TabIndex        =   37
            Top             =   4620
            Width           =   2292
         End
         Begin VB.Label lbl78 
            Caption         =   "1"
            Height          =   255
            Left            =   8160
            TabIndex        =   36
            Top             =   4440
            Visible         =   0   'False
            Width           =   375
         End
         Begin VB.Label lbl76 
            Caption         =   "lbl76"
            Height          =   255
            Left            =   7440
            TabIndex        =   35
            Top             =   4440
            Visible         =   0   'False
            Width           =   495
         End
         Begin VB.Label lbl250 
            Caption         =   " HB9ZS/2008"
            Height          =   252
            Left            =   8520
            TabIndex        =   34
            Top             =   8400
            Width           =   1212
         End
         Begin VB.Label lbl225 
            Alignment       =   1  'Rechts
            Height          =   252
            Left            =   7580
            TabIndex        =   33
            Top             =   7000
            Width           =   372
         End
         Begin VB.Label lbl224 
            Caption         =   "1269"
            Height          =   252
            Left            =   6450
            TabIndex        =   32
            Top             =   7000
            Width           =   1932
         End
         Begin VB.Label lbl05 
            Caption         =   "1204"
            Height          =   255
            Left            =   480
            TabIndex        =   31
            Top             =   900
            Visible         =   0   'False
            Width           =   255
         End
         Begin VB.Label lbl04 
            Caption         =   "1203"
            Height          =   255
            Left            =   360
            TabIndex        =   30
            Top             =   520
            Width           =   495
         End
         Begin VB.Label lbl03 
            Caption         =   "lbl03"
            Height          =   252
            Left            =   7560
            TabIndex        =   29
            Top             =   7440
            Visible         =   0   'False
            Width           =   852
         End
         Begin VB.Label lbl14 
            Caption         =   "lbl14"
            Height          =   252
            Left            =   7680
            TabIndex        =   28
            Top             =   3360
            Visible         =   0   'False
            Width           =   732
         End
         Begin VB.Label lbl01 
            Caption         =   "1002"
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
            Left            =   840
            TabIndex        =   27
            Top             =   240
            Width           =   5775
         End
         Begin VB.Label lbl63 
            Height          =   255
            Left            =   4560
            TabIndex        =   26
            Top             =   2610
            Width           =   495
         End
         Begin VB.Label lbl41 
            Height          =   255
            Left            =   3000
            TabIndex        =   25
            Top             =   2040
            Width           =   495
         End
         Begin VB.Label lbl62 
            Alignment       =   1  'Rechts
            Height          =   255
            Left            =   3720
            TabIndex        =   24
            Top             =   2640
            Width           =   735
         End
         Begin VB.Label lbl232 
            Alignment       =   1  'Rechts
            Height          =   255
            Left            =   3720
            TabIndex        =   23
            Top             =   8220
            Width           =   735
         End
      End
      Begin VB.Frame frmK1 
         BorderStyle     =   0  'Kein
         Height          =   8292
         Left            =   -73800
         TabIndex        =   1
         Top             =   1080
         Width           =   8172
         Begin VB.Frame Frame2 
            BorderStyle     =   0  'Kein
            Caption         =   "Frame2"
            Height          =   7932
            Left            =   120
            TabIndex        =   118
            Top             =   240
            Width           =   5772
            Begin VB.ComboBox cboK51 
               Height          =   315
               ItemData        =   "frmMain.frx":4BF4
               Left            =   4035
               List            =   "frmMain.frx":4BF6
               MouseIcon       =   "frmMain.frx":4BF8
               MousePointer    =   99  'Benutzerdefiniert
               Style           =   2  'Dropdown-Liste
               TabIndex        =   122
               ToolTipText     =   "1419"
               Top             =   1080
               Width           =   855
            End
            Begin VB.ComboBox cboK31 
               Height          =   288
               ItemData        =   "frmMain.frx":4F02
               Left            =   3560
               List            =   "frmMain.frx":4F04
               MouseIcon       =   "frmMain.frx":4F06
               MousePointer    =   99  'Benutzerdefiniert
               Style           =   2  'Dropdown-Liste
               TabIndex        =   121
               ToolTipText     =   "1417"
               Top             =   1560
               Width           =   1335
            End
            Begin VB.ComboBox cboK131 
               Height          =   288
               Left            =   3600
               MouseIcon       =   "frmMain.frx":5210
               MousePointer    =   99  'Benutzerdefiniert
               TabIndex        =   120
               Text            =   "cboK131"
               Top             =   2160
               Width           =   1335
            End
            Begin VB.ComboBox cboK231 
               Height          =   288
               ItemData        =   "frmMain.frx":551A
               Left            =   3600
               List            =   "frmMain.frx":551C
               MouseIcon       =   "frmMain.frx":551E
               MousePointer    =   99  'Benutzerdefiniert
               Style           =   2  'Dropdown-Liste
               TabIndex        =   119
               Top             =   2760
               Width           =   1335
            End
            Begin MSFlexGridLib.MSFlexGrid FlexKabel2 
               Height          =   6132
               Left            =   120
               TabIndex        =   123
               Top             =   240
               Width           =   4932
               _ExtentX        =   8705
               _ExtentY        =   10821
               _Version        =   393216
               ScrollBars      =   0
            End
         End
         Begin VB.ListBox lstK22 
            Enabled         =   0   'False
            Height          =   255
            Left            =   7080
            TabIndex        =   115
            Top             =   5040
            Visible         =   0   'False
            Width           =   732
         End
         Begin VB.ListBox lstK21 
            Enabled         =   0   'False
            Height          =   255
            Left            =   6240
            TabIndex        =   114
            Top             =   5040
            Visible         =   0   'False
            Width           =   732
         End
         Begin VB.ListBox lstK12 
            Enabled         =   0   'False
            Height          =   255
            Left            =   7080
            TabIndex        =   9
            Top             =   4680
            Visible         =   0   'False
            Width           =   735
         End
         Begin VB.ListBox lstK11 
            Enabled         =   0   'False
            Height          =   255
            ItemData        =   "frmMain.frx":5828
            Left            =   6240
            List            =   "frmMain.frx":582A
            TabIndex        =   8
            Top             =   4680
            Visible         =   0   'False
            Width           =   732
         End
         Begin VB.CommandButton cmdK1 
            Caption         =   "1415"
            Height          =   375
            Left            =   6480
            TabIndex        =   7
            Top             =   6840
            Visible         =   0   'False
            Width           =   1335
         End
         Begin VB.Frame frm11 
            Caption         =   "1416"
            Height          =   3735
            Left            =   6240
            TabIndex        =   2
            Top             =   840
            Width           =   1575
            Begin VB.ListBox lstK1 
               Enabled         =   0   'False
               Height          =   255
               Left            =   120
               TabIndex        =   4
               TabStop         =   0   'False
               Top             =   480
               Width           =   615
            End
            Begin VB.ListBox lstK2 
               Enabled         =   0   'False
               Height          =   255
               Left            =   720
               TabIndex        =   3
               TabStop         =   0   'False
               Top             =   480
               Width           =   735
            End
            Begin VB.Label LblK11 
               Alignment       =   2  'Zentriert
               Caption         =   "f"
               Height          =   255
               Left            =   120
               TabIndex        =   6
               Top             =   240
               Width           =   615
            End
            Begin VB.Label lblK12 
               Caption         =   "dB/100m"
               Height          =   255
               Left            =   720
               TabIndex        =   5
               Top             =   240
               Width           =   735
            End
         End
         Begin VB.Label lblK261 
            Alignment       =   1  'Rechts
            Height          =   252
            Left            =   3840
            TabIndex        =   116
            Top             =   5640
            Width           =   675
         End
         Begin VB.Label lblK263 
            ForeColor       =   &H000000FF&
            Height          =   252
            Left            =   3600
            TabIndex        =   113
            Top             =   5640
            Width           =   948
         End
         Begin VB.Label lblK163 
            ForeColor       =   &H000000FF&
            Height          =   252
            Left            =   3600
            TabIndex        =   112
            Top             =   3960
            Width           =   948
         End
         Begin VB.Label lblK162 
            ForeColor       =   &H000000FF&
            Height          =   255
            Left            =   3600
            TabIndex        =   111
            Top             =   2280
            Width           =   950
         End
         Begin VB.Label lblK1 
            Caption         =   "lblK1"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   252
            Left            =   6360
            TabIndex        =   16
            Top             =   360
            Visible         =   0   'False
            Width           =   1452
         End
         Begin VB.Label lblK161 
            Alignment       =   1  'Rechts
            Caption         =   "0"
            Height          =   255
            Left            =   3840
            TabIndex        =   15
            Top             =   3960
            Width           =   675
         End
         Begin VB.Label lblK52 
            Caption         =   "lblK52"
            Height          =   252
            Left            =   7080
            TabIndex        =   14
            Top             =   5640
            Visible         =   0   'False
            Width           =   612
         End
         Begin VB.Label lblK97 
            Caption         =   "0"
            Height          =   252
            Left            =   7080
            TabIndex        =   13
            Top             =   6480
            Visible         =   0   'False
            Width           =   372
         End
         Begin VB.Label LblK98 
            Caption         =   "0"
            Height          =   252
            Left            =   7080
            TabIndex        =   12
            Top             =   6240
            Visible         =   0   'False
            Width           =   372
         End
         Begin VB.Label lblK99 
            Caption         =   "0"
            Height          =   252
            Left            =   7080
            TabIndex        =   11
            Top             =   5880
            Visible         =   0   'False
            Width           =   372
         End
         Begin VB.Label lblK61 
            Alignment       =   1  'Rechts
            Caption         =   "0"
            Height          =   255
            Left            =   3800
            TabIndex        =   10
            Top             =   2280
            Width           =   735
         End
      End
      Begin VB.Label lblK2 
         Caption         =   "Kabeldaten"
         Height          =   252
         Left            =   -66840
         TabIndex        =   94
         Top             =   960
         Visible         =   0   'False
         Width           =   972
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'=============================================
'Formular Feldstärke mit den Registern
'Berechnung, Koaxialkabel, Antennendaten
'ubernimmt die Antennendaten aus dem Ordner
'Ant_Dat. diese Dateien sind als Textfile
'abgelegt und somit editierbar
' 12.09.2004 / hb9zs
'=============================================

Option Explicit

Public txpos As String
Private Declare Function OSWinHelp% Lib "user32" Alias "WinHelpA" (ByVal hwnd&, ByVal HelpFile$, ByVal wCommand%, dwData As Any)
    Const inch = 1440 'Twips per Inch
    Dim Antennen(14, 14, 29) As Variant
    Dim Kabel(20, 30) As Variant

Private Stay As Integer ' Zustand für Timerereignis
    
'=============

Dim mZ As Integer ' Modulationsfaktor
Dim Ascii As String
Dim Asci As String

Private YH As Long

Dim Status2 As Integer
Dim Funtionstaste As Integer


Public Sub SelectAll(TxtBox As TextBox)

'Einfügemarke für Text in den Eingabefeldern
    With TxtBox
        .SelStart = 0
        .SelLength = Len(.Text)
    End With
    
End Sub




Private Sub Timer2_Timer()

    
    Static Zeit As Integer
    
    If Timer2.Enabled = True Then
    
        If Zeit Then
  
        Else
            'wenn Zeit abgelaufen
            
            Status2 = 0     'Timer ausschalten
                    
            If Status2 = 0 Then
                Timer2.Enabled = False
                
                If Funtionstaste = 118 Then  'F7 nach rechts verschieben'
                        If FlexGrid2.LeftCol = 5 Then
                        Else
                            FlexGrid2.LeftCol = FlexGrid2.LeftCol + 1
                        End If
                End If
                
                If Funtionstaste = 117 Then  'F6 nach links verschieben
                        If FlexGrid2.LeftCol = 1 Then
                        Else
                            FlexGrid2.LeftCol = FlexGrid2.LeftCol - 1
                        End If
                End If

                
            End If

        
        End If
    
        Zeit = Not Zeit

    End If

End Sub


Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)

'Funktionstasten belegen

    'Berechnen wenn F4 gedrückt wird
    If KeyCode = 115 Then
        NIS_berechnen
    End If
 
    If KeyCode = 118 Then   'nach rechts verschieben
        If FlexGrid2.LeftCol = 5 Then
            Exit Sub
        Else
            Funtionstaste = KeyCode 'Für Timer umwandeln
 '           Kolonneverschieben      'Sub
            Timer2.Enabled = True   'Timer starten
            Status2 = 1             'Timer Status
        End If
    End If
    
    If KeyCode = 117 Then  'nach links verschieben
        If FlexGrid2.LeftCol = 1 Then
            Exit Sub
        Else
            Funtionstaste = KeyCode 'Für Timer umwandeln
 '           Kolonneverschieben      'Sub
            Timer2.Enabled = True   'Timer starten
            Status2 = 1             'Timer Status
        End If
    End If

 
End Sub

Private Sub cmd1_Click()

    'Kolonne löschen
    Dim Reihe As Integer
    
    For Reihe = 0 To 27
    fGrid1.Flex1.TextMatrix(Reihe, FlexGrid2.LeftCol + 3) = ""
    Next Reihe

End Sub


'=====================================================neu

Private Sub Timer1_Timer()

'Blinker für die rote Berechnungsanzeige
    
    Static Zeit As Integer
    'blk 1 Start vom Klassenmodul aus bei Aenderungen im MSFlexGrid2
    'Stay 1 Start von Aenderungen im Text
    If Stay = 1 Or blk = 1 Then
    
    If Zeit Then
        cmd03.BackColor = RGB(255, 0, 0)
    Else
        cmd03.BackColor = &HC0C0C0
    End If
        Zeit = Not Zeit

    End If

End Sub

Private Sub txt02_KeyPress(KeyAscii As Integer)

    'Schrift in Grossbuchstaben umwandeln (Rufzeichen)
    Select Case KeyAscii
    Case Asc("a") To Asc("z")
        KeyAscii = KeyAscii - 32
    End Select

End Sub


Private Sub Form_Load()

    'neu==============================

    C1 = 1      'MSFlex2 Kolonne auf 1 setzen
    Stay = 0    'Blinker für Berechnung anhalten
    blk = 0     'Blinker für Berechnung anhalten
        
'=====
    cboF1.AddItem "1.8"
    cboF1.ItemData(cboF1.NewIndex) = 18     '615
    cboF1.AddItem "3.5"
    cboF1.ItemData(cboF1.NewIndex) = 35     '446
    cboF1.AddItem "7"
    cboF1.ItemData(cboF1.NewIndex) = 70     '324
    cboF1.AddItem "10"
    cboF1.ItemData(cboF1.NewIndex) = 100    '280
    cboF1.AddItem "14"
    cboF1.ItemData(cboF1.NewIndex) = 140    '280
    cboF1.AddItem "18"
    cboF1.ItemData(cboF1.NewIndex) = 180    '280
    cboF1.AddItem "21"
    cboF1.ItemData(cboF1.NewIndex) = 210    '280
    cboF1.AddItem "24"
    cboF1.ItemData(cboF1.NewIndex) = 240    '280
    cboF1.AddItem "28"
    cboF1.ItemData(cboF1.NewIndex) = 280    '280
    cboF1.AddItem "50"
    cboF1.ItemData(cboF1.NewIndex) = 500    '280
    cboF1.AddItem "144"
    cboF1.ItemData(cboF1.NewIndex) = 1440   '280
    cboF1.AddItem "432"
    cboF1.ItemData(cboF1.NewIndex) = 4320   '286
    cboF1.AddItem "1240"
    cboF1.ItemData(cboF1.NewIndex) = 12400  '485
    cboF1.AddItem "2300"
    cboF1.ItemData(cboF1.NewIndex) = 23000  '610
    cboF1.AddItem "5650"
    cboF1.ItemData(cboF1.NewIndex) = 56500 '610
    cboF1.AddItem "10000"
    cboF1.ItemData(cboF1.NewIndex) = 100000 '610

    cboK51.AddItem "1.8"
    cboK51.ItemData(cboK51.NewIndex) = 1    '18     '615
    cboK51.AddItem "3.5"
    cboK51.ItemData(cboK51.NewIndex) = 2    '35     '446
    cboK51.AddItem "7"
    cboK51.ItemData(cboK51.NewIndex) = 3    '70     '324
    cboK51.AddItem "10"
    cboK51.ItemData(cboK51.NewIndex) = 4    '100    '280
    cboK51.AddItem "14"
    cboK51.ItemData(cboK51.NewIndex) = 5    '140    '280
    cboK51.AddItem "18"
    cboK51.ItemData(cboK51.NewIndex) = 6    '180    '280
    cboK51.AddItem "21"
    cboK51.ItemData(cboK51.NewIndex) = 7    '210    '280
    cboK51.AddItem "24"
    cboK51.ItemData(cboK51.NewIndex) = 8    '240    '280
    cboK51.AddItem "28"
    cboK51.ItemData(cboK51.NewIndex) = 9    '280    '280
    cboK51.AddItem "50"
    cboK51.ItemData(cboK51.NewIndex) = 10   '500    '280
    cboK51.AddItem "144"
    cboK51.ItemData(cboK51.NewIndex) = 11   '1440   '280
    cboK51.AddItem "432"
    cboK51.ItemData(cboK51.NewIndex) = 12   '4320   '286
    cboK51.AddItem "1240"
    cboK51.ItemData(cboK51.NewIndex) = 13   '12400  '485
    cboK51.AddItem "2300"
    cboK51.ItemData(cboK51.NewIndex) = 14   '24000  '610
    cboK51.AddItem "5650"
    cboK51.ItemData(cboK51.NewIndex) = 15   '100000 '610
    cboK51.AddItem "10000"
    cboK51.ItemData(cboK51.NewIndex) = 16   '100000 '610
    
    cboA1.AddItem "1.8"
    cboA1.ItemData(cboA1.NewIndex) = 1  '18     '615
    cboA1.AddItem "3.5"
    cboA1.ItemData(cboA1.NewIndex) = 2  '35     '446
    cboA1.AddItem "7"
    cboA1.ItemData(cboA1.NewIndex) = 3  '70     '324
    cboA1.AddItem "10"
    cboA1.ItemData(cboA1.NewIndex) = 4  '100    '280
    cboA1.AddItem "14"
    cboA1.ItemData(cboA1.NewIndex) = 5  '140    '280
    cboA1.AddItem "18"
    cboA1.ItemData(cboA1.NewIndex) = 6  '180    '280
    cboA1.AddItem "21"
    cboA1.ItemData(cboA1.NewIndex) = 7  '210    '280
    cboA1.AddItem "24"
    cboA1.ItemData(cboA1.NewIndex) = 8  '240    '280
    cboA1.AddItem "28"
    cboA1.ItemData(cboA1.NewIndex) = 9  '280    '280
    cboA1.AddItem "50"
    cboA1.ItemData(cboA1.NewIndex) = 10 '500    '280
    cboA1.AddItem "144"
    cboA1.ItemData(cboA1.NewIndex) = 11 '1440   '280
    cboA1.AddItem "432"
    cboA1.ItemData(cboA1.NewIndex) = 12 '4320   '286
    cboA1.AddItem "1240"
    cboA1.ItemData(cboA1.NewIndex) = 13 '12400  '485
    cboA1.AddItem "2300"
    cboA1.ItemData(cboA1.NewIndex) = 14 '24000  '610
    cboA1.AddItem "5650"
    cboA1.ItemData(cboA1.NewIndex) = 15 '100000 '610
    cboA1.AddItem "10000"
    cboA1.ItemData(cboA1.NewIndex) = 16 '100000 '610


'=====
        
    Berechnung_initialisieren   'Sub

    Berechnung_spezifizieren    'Sub

    Textladen_Berechnung        'Sub
    
    cboA1_sperren = 0  'Freigeben


    lblK2.Caption = "Kabelxdaten"    'Filename für Kabeldaten laden
    ProgressBar1.Visible = False    'Auf Flexgrid Antennendaten unterdrücken

    'Blinker für cmd03 Taste
    Timer1.Interval = 500
    Timer1.Enabled = True

    txt03.Text = GetSetting(App.Title, "Angaben", "Name", "")
    txt04.Text = GetSetting(App.Title, "Angaben", "Strasse", "")
    txt05.Text = GetSetting(App.Title, "Angaben", "Ort", "")

    'cboF  Feldstärke - Grenzwerte (zur Erinnerung)
    ' "1.8" 647
    ' "3.5" 465
    ' "7" 324
    ' "10" 280
    ' "14" 280
    ' "18" 280
    ' "21" 280
    ' "24" 280
    ' "28" 280
    ' "50" 280
    ' "144" 280
    ' "430" 286
    ' "1240" 485
    ' "2400" 610
    ' "5650" 610
    ' "10000" 610
    'Initialisieren

    FlexKabel1.Cols = 10 '11    ' Neu in Formload spezifiziert
    FlexKabel1.Rows = 17 '16 '15        ' Neu in Formload spezifiziert
    
    DoTextDef                'Hinweistext
    DoKabeldaten_load        'Kabeldaten laden ===> neu
    DoKabeldatenuebertragen  'Kabeldaten von Flexgrid in Matrix einfügen

    DoAntHerstTyp            'Antennen Flexgrid Hersteller, Typen definieren
    DoAntennenDef            'Raster von Antennen Felxgrid festlegen
 
    'Def der Combox-Ausgangswerte
    cboK51.ListIndex = 0
      
    'Berechnungstabelle anzeigen auf Bildschirm
    fGrid1.Show
 
    'Berechnung im Kabelformular auslösen, anstelle der Taste cmdA1
'    lblK97.Caption = 1

    'Textfelder mit 0 Belegen
    txtA2.Text = 0
    txtA6.Text = 0
    
    Kabel_definieren  'Sub neu für KabelFlex

    

End Sub


Private Sub Combo_laden()

    'Gebäudedämpfung spezifizieren
        
    Dim Tex1 As String
    Tex1 = LoadResString(1311 + RS)
    
    'Inhalt ComoBox
    cboG1.AddItem LoadResString(1311 + RS), 0
    cboG1.ItemData(cboG1.NewIndex) = 0
    cboG1.AddItem LoadResString(1312 + RS), 1
    cboG1.ItemData(cboG1.NewIndex) = 10
    cboG1.AddItem LoadResString(1313 + RS), 2
    cboG1.ItemData(cboG1.NewIndex) = 10
    cboG1.AddItem LoadResString(1314 + RS), 3
    cboG1.ItemData(cboG1.NewIndex) = 0
    cboG1.AddItem LoadResString(1315 + RS), 4
    cboG1.ItemData(cboG1.NewIndex) = 10
    cboG1.AddItem LoadResString(1316 + RS), 5
    cboG1.ItemData(cboG1.NewIndex) = 5
    cboG1.AddItem LoadResString(1317 + RS), 6
    cboG1.ItemData(cboG1.NewIndex) = 0
    
    'Auf ersten Eintrag setzen
    cboG1.ListIndex = 0
    
    'Platzierung ComboBox
    cboG1.Top = Frame1.Top + FlexGrid1.Top + 10 + (19 * FlexGrid1.CellHeight)

    'Flexgrid auf erste Kolonne setzen
'    FlexGrid2.LeftCol = 1


End Sub


Private Sub cboG1_Click()

    'Gebäudedämpfung Auswahl
    Dim kol As Integer
    
    kol = FlexGrid2.LeftCol
    FlexGrid2.TextMatrix(18, kol) = cboG1.ItemData(cboG1.ListIndex)
    FlexGrid2.TextMatrix(18, kol) = Format(FlexGrid2.TextMatrix(18, kol), "0.00")
    'Listindex speichern
    FlexGrid2.TextMatrix(24, kol) = cboG1.ListIndex
    
    Stay = 1 'Blinker Berechnung starten

End Sub


Sub DoTextDef()

    'Spezifikation sämtlicher Textfelder und Hilfetext
    frmMain.Caption = LoadResString(1117 + RS)


    Immissionsberechnung.TabCaption(0) = LoadResString(1201 + RS)
    txt02.ToolTipText = LoadResString(1208 + RS)
    txt03.ToolTipText = LoadResString(1205 + RS)
    txt04.ToolTipText = LoadResString(1206 + RS)
    txt05.ToolTipText = LoadResString(1207 + RS)

    lbl01.Caption = LoadResString(1202 + RS)
    lbl04.Caption = LoadResString(1203 + RS)
    lbl05.Caption = LoadResString(1204 + RS)
    'lbl06.Caption = LoadResString(1209 + RS)
    'lbl07.Caption = LoadResString(1220 + RS)

    cmd03.Caption = LoadResString(1276 + RS)
    lbl224.Caption = LoadResString(1269 + RS)
    cmd03.ToolTipText = LoadResString(1302 + RS)
    lbl250.ToolTipText = LoadResString(1306 + RS)
    lbl301.Caption = LoadResString(1750 + RS)
    lbl302.Caption = LoadResString(1751 + RS)
    lbl24.Caption = LoadResString(1723 + RS)
    lbl34.Caption = LoadResString(1724 + RS)
    'Koaxialkabelfeld
    Immissionsberechnung.TabCaption(1) = LoadResString(1401 + RS)
    'lblK0 = LoadResString(1402 + RS)
    'lblK5 = LoadResString(1403 + RS)
    'lblK3 = LoadResString(1404 + RS)
    'lblK4 = LoadResString(1405 + RS)
    'lblK6 = LoadResString(1406 + RS)
    'lblK7 = LoadResString(1407 + RS)
    'lblK10 = LoadResString(1408 + RS)
    'lblK112 = LoadResString(1409 + RS)
    'lblK113 = LoadResString(1410 + RS)
    'lblK114 = LoadResString(1411 + RS)
    'lblK115 = LoadResString(1412 + RS)

    'lblK210 = LoadResString(1431 + RS)
    'lblK212 = LoadResString(1432 + RS)
    'lblK213 = LoadResString(1433 + RS)
    'lblK214 = LoadResString(1434 + RS)

    'lblK8 = LoadResString(1413 + RS)
    'lblK9 = LoadResString(1414 + RS)
    cmdK1.Caption = LoadResString(1415 + RS)
    frm11.Caption = LoadResString(1416 + RS)
    cboK31.ToolTipText = LoadResString(1417 + RS)
    'txtK41.ToolTipText = LoadResString(1418 + RS)
    cboK51.ToolTipText = LoadResString(1419 + RS)
    'txtK80.ToolTipText = LoadResString(1420 + RS)
    'txtK81.ToolTipText = "xx.x" 'LoadResString(1420 + RS)
    'txtK82.ToolTipText = LoadResString(1420 + RS)
    'txtK83.ToolTipText = "xx.x" 'LoadResString(1420 + RS)
    'txtK80.Text = LoadResString(1758 + RS)
    Immissionsberechnung.TabCaption(2) = LoadResString(1501 + RS)
    frmA2.Caption = LoadResString(1502 + RS)
    frmA3.Caption = LoadResString(1503 + RS)
    frm4.Caption = LoadResString(1504 + RS)
    lblA9 = LoadResString(1505 + RS)
    frmA5.Caption = LoadResString(1506 + RS)
    Frame3.Caption = LoadResString(1507 + RS)
    Label1 = LoadResString(1508 + RS)
    lblA20 = LoadResString(1509 + RS)
    frmA6.Caption = LoadResString(1510 + RS)
    cboA1.ToolTipText = LoadResString(1511 + RS)
    flexH1.ToolTipText = LoadResString(1512 + RS)
    flexT1.ToolTipText = LoadResString(1513 + RS)
    txtA2.ToolTipText = LoadResString(1514 + RS)
    txtA6.ToolTipText = LoadResString(1515 + RS)
    txtA7.ToolTipText = LoadResString(1516 + RS)
    flexA1.ToolTipText = LoadResString(1517 + RS)
    cmdA1.Caption = LoadResString(1518 + RS)
    lblA21.Caption = LoadResString(1752 + RS)
    lblA22.Caption = LoadResString(1753 + RS)
    lblA23.Caption = LoadResString(1754 + RS)
    lblA24.Caption = LoadResString(1754 + RS)
    lblA25.Caption = LoadResString(1755 + RS)
    lblA26.Caption = LoadResString(1755 + RS)
    lblA19.Caption = LoadResString(1756 + RS)
    cmdK2.Caption = LoadResString(1421 + RS)
    cmd1.Caption = LoadResString(1308 + RS)


End Sub

Sub DoKabelDef()

'Kabel-Typen in Kabeltabelle einfügen

    cboK31.AddItem "Aircom plus", 0
    cboK31.AddItem "Aircell 7", 1
    cboK31.AddItem "H100", 2
    cboK31.AddItem "H2000", 3
    cboK31.AddItem "RG-58", 4
    cboK31.AddItem "RG-174", 5
    cboK31.AddItem "RG-213", 6
    cboK31.AddItem "RG-220", 7
    
    cboK31.ListIndex = 0
    
    cboK131.AddItem "Aircom plus", 0
    cboK131.AddItem "Aircell 7", 1
    cboK131.AddItem "H100", 2
    cboK131.AddItem "H2000", 3
    cboK131.AddItem "RG-58", 4
    cboK131.AddItem "RG-174", 5
    cboK131.AddItem "RG-213", 6
    cboK131.AddItem "RG-220", 7
    
    cboK131.ListIndex = 0


End Sub



Sub DoAntHerstTyp()

On Error Resume Next
'Antennen: Flexgrid Hersteller und Typen definieren

    flexH1.Rows = 2
    flexH1.Cols = 3
    flexH1.ColWidth(0) = 0
    flexH1.ColWidth(1) = 0  '300
    flexH1.ColWidth(2) = 1000
   
    flexT1.Rows = 1
    flexT1.Cols = 3
    flexT1.ColWidth(0) = 0
    flexT1.ColWidth(1) = 0  '300
    flexT1.ColWidth(2) = 1200
    
' Zellen Focus(kein Rahmen)Farbe in Eigenschaften festlegen
' Zelle mit Farbe hinterlegen
    flexH1.FocusRect = 0
    flexT1.FocusRect = 0

 'Text : Row 0, Col 1 und 2 einfügen
    
 'Flexgrid H1 laden
    lblh14.Caption = Int(Rnd * 100) + 2
    
    If RS = 0 Then
        flexH1.TextMatrix(0, 1) = "Nr"
        flexH1.TextMatrix(0, 2) = "Hersteller"
        flexT1.TextMatrix(0, 1) = "Nr"
        flexT1.TextMatrix(0, 2) = "Typ"
    End If
    
    If RS = 1000 Then
        flexH1.TextMatrix(0, 1) = "No"
        flexH1.TextMatrix(0, 2) = "Constructeur"
        flexT1.TextMatrix(0, 1) = "No."
        flexT1.TextMatrix(0, 2) = "Type"
    End If

 'Erste Spalte schmaler einstellen.
     flexH1.ColAlignment(0) = 1   ' Zentrieren.
     flexT1.ColAlignment(2) = 0   ' links
  
   
 


End Sub

Sub DoAntennenDef()

'Flex Grid Antennen (flexA1) definieren
 
    Dim k As Integer, t As Integer
    Dim Zeile As Variant, Spalte As Variant
    
    
'Pfadangabe für File laden

    txtA3 = App.Path & "\" & flexA1.TextMatrix(1, 2) & ".ant"
    
'Flexgrid Antennen und Winkeldämpfung definieren

    flexA1.Rows = 17 '16    '15
    flexA1.Cols = 15
    flexA1.ColWidth(0) = 0
    flexA1.ColWidth(1) = 1550
    flexA1.ColWidth(2) = 1100
    flexA1.ColWidth(3) = 600
    flexA1.ColWidth(4) = 550
    flexA1.ColWidth(5) = 550
    flexA1.ColWidth(6) = 550
    flexA1.ColWidth(7) = 550
    flexA1.ColWidth(8) = 550
    flexA1.ColWidth(9) = 550
    flexA1.ColWidth(10) = 550
    flexA1.ColWidth(11) = 550
    flexA1.ColWidth(12) = 550
    flexA1.ColWidth(13) = 550
    flexA1.ColWidth(14) = 550
    
'Zellen Focus(kein Rahmen)Farbe in Eigenschaften festlegen
    flexA1.FocusRect = 0

'Flexgrid füllen
    Dim Y As Integer
    For Y = 1 To 16 '15 '14
        'Flexgrid Kolonne 3 mit Frequenz füllen
        flexA1.TextMatrix(Y, 3) = (cboA1.List(Y - 1))
    Next Y

 'Text : Row 0, Col 1 - 14 einfügen
    If RS = 0 Then
    flexA1.TextMatrix(0, 1) = "Hersteller"
    flexA1.TextMatrix(0, 2) = "Typ"
    End If
    
    If RS = 1000 Then
    flexA1.TextMatrix(0, 1) = "Constructeur"
    flexA1.TextMatrix(0, 2) = "Type"
    End If
    
    flexA1.TextMatrix(0, 3) = "f(MHz)"
    flexA1.TextMatrix(0, 4) = "dBi"
    flexA1.TextMatrix(0, 5) = "  0° "
    flexA1.TextMatrix(0, 6) = "  10° "
    flexA1.TextMatrix(0, 7) = "  20° "
    flexA1.TextMatrix(0, 8) = "  30° "
    flexA1.TextMatrix(0, 9) = "  40° "
    flexA1.TextMatrix(0, 10) = "  50° "
    flexA1.TextMatrix(0, 11) = "  60° "
    flexA1.TextMatrix(0, 12) = "  70° "
    flexA1.TextMatrix(0, 13) = "  80° "
    flexA1.TextMatrix(0, 14) = "  90° "
    
    txtA8.Text = LoadResString(1757 + RS)
    txtA9.Text = LoadResString(1757 + RS)

    'Titelleiste fett schreiben
    Dim xc As Integer
    For xc = 1 To 14
    flexA1.Row = 0
    flexA1.Col = xc
    flexA1.CellFontBold = True
    Next



'Frequenz cboA1 auf 0 setzen
'    cboA1.ListIndex = 0



End Sub

Private Sub Forms_Unload(Cancel As Integer)
      
    'Klassenmodul entladen
    Set clsFGEdit = Nothing
    Set clsFGEdit2 = Nothing

    
        SaveSetting App.Title, "Angaben", "Name", txt03.Text
        SaveSetting App.Title, "Angaben", "Strasse", txt04.Text
        SaveSetting App.Title, "Angaben", "Ort", txt05.Text
        
    Dim i As Integer

    'close all sub forms
    For i = Forms.Count - 1 To 1 Step -1
        Unload Forms(i)
    Next
    
    If Me.WindowState <> vbMinimized Then
        SaveSetting App.Title, "Settings", "MainLeft", Me.Left
        SaveSetting App.Title, "Settings", "MainTop", Me.Top
        SaveSetting App.Title, "Settings", "MainWidth", Me.Width
        SaveSetting App.Title, "Settings", "MainHeight", Me.Height
    End If
        
   
End Sub


Private Sub lbl232_change()

    'Rote Berechnungsanzeige
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub lbl62_change()

    'Rote Berechnungsanzeige
    lbl76.Caption = Int(Rnd * 100) + 2
    
End Sub


Private Sub lbl76_change()
    
    If cboA1_sperren = 0 Then
    
    'Taste Berechnen rot unterlegen
    cmd03.BackColor = RGB(255, 0, 0)
    Stay = 1
    
    End If
    
End Sub


Private Sub txt25_Change()
    
'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub txt25_Click()

    SelectAll txt25

End Sub


Private Sub txt25_GotFocus()

    SelectAll txt25
    
End Sub


Private Sub txt35_Change()
    
'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub txt35_Click()

    SelectAll txt35

End Sub


Private Sub txt35_GotFocus()

    SelectAll txt35
    
End Sub


Private Sub txt301_Change()
    
    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub txt301_Click()

    SelectAll txt301

End Sub


Private Sub txt301_GotFocus()

    SelectAll txt301
    
End Sub


Private Sub txt302_Change()
    
    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub txt302_Click()

    SelectAll txt302

End Sub


Private Sub txt302_GotFocus()

    SelectAll txt302
    
End Sub


Private Sub txtA19_Change()

    lbl124.Caption = txtA19.Text

End Sub


Private Sub lblK91_Change()
    
    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub



'neuer Teil
'======================================================================neu

Private Sub Berechnung_initialisieren()

    'FlexGrid2 spezifizieren
    FlexGrid2.Rows = 29
    FlexGrid2.Cols = 6
    FlexGrid2.ColWidth(0) = 0
    FlexGrid2.RowHeightMin = 280
    FlexGrid2.Height = FlexGrid2.RowHeight(1) * 26 - 150
  
    
    FlexGrid2.Width = 945
    'Kolonnenbreite festlegen
    Dim X As Integer
    For X = 1 To 5
        FlexGrid2.ColWidth(X) = 870
    Next X
    
    'FlexGrid1 spezifizieren
    FlexGrid1.Rows = 24
    FlexGrid1.Cols = 5
    FlexGrid1.Width = 5500
    FlexGrid1.ColWidth(0) = 10
    FlexGrid1.ColWidth(1) = 3300
    FlexGrid1.ColWidth(2) = 650
    FlexGrid1.ColWidth(3) = 700
    FlexGrid1.ColWidth(4) = 900
    FlexGrid1.RowHeightMin = 280
    'Totale Höhe von FlexGrid 1 festlegen
    FlexGrid1.Height = FlexGrid1.RowHeight(1) * 24 + 30
    
    
        'Gebäudedämpfung Auswahl
    Dim kol As Integer
    
    'Alle Kolonnen mit Gebäudedämpfung füllen
    For kol = 1 To 5
        FlexGrid2.TextMatrix(24, kol) = 0
    Next

    
    'Position von Klammer und Text Horizontalprojektion festlegen
    Line1.y1 = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 1.5) + 20
    Line1.y2 = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 3.5) + 20
    
    Line2.y1 = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 2.5) + 20
    Line2.y2 = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 2.5) + 20
    
    Line3.y1 = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 1.5) + 20
    Line3.y2 = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 1.5) + 20
    
    Line4.y1 = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 3.5) + 20
    Line4.y2 = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 3.5) + 20
    
    lbl24.Top = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 1.5)
    txt25.Top = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 1.5) - 60
    
    lbl34.Top = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 3) - 40
    txt35.Top = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 3) - 80
    
    
    'Position von AntennenHersteller und Typ festlegen (Reihe 11)
    lbl124.Top = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 12) + 70
    
    'Position von Sicherheitsabstand festlegen (Reihe 21)
    lbl224.Top = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 21) + 70
    lbl225.Top = Frame1.Top + FlexGrid1.Top + (FlexGrid1.RowHeight(1) * 21) + 70
    
    'Frequenzauswahl plazieren
    cboF1.Top = Frame1.Top + FlexGrid2.Top + 25
    
    'Betriebsartwahl plazieren
    cboM1.Top = Frame1.Top + FlexGrid2.Top + FlexGrid2.RowHeight(1) * 4 + 20
    
    
    'Erste Kolonne von FlexGrid2 anzeigen
    C1 = 1
    
    'oberste Reihe weiss statt grau
    FlexGrid1.Row = 0
    FlexGrid1.CellBackColor = &H80000005  'weiss

    Dim Reihe As Integer
    Dim Spalte As Integer
    
    'Flexgrid 2, unterste letze Reihe schwarz füllen
    For Spalte = 1 To 5
            FlexGrid2.Row = 24
            FlexGrid2.Col = Spalte
            FlexGrid2.CellBackColor = &H80000007
        Next Spalte
    
    
    'FlexGrid1 Erste Reihe weiss füllen
    For Spalte = 1 To 4
            FlexGrid1.Row = 0
            FlexGrid1.Col = Spalte
            FlexGrid1.CellBackColor = &H80000005
        Next Spalte
    
    
    'MSHFlexGrid Felder für die "Eingabe" gelb markieren
    For Reihe = 0 To 3
        For Spalte = 1 To 5
            FlexGrid2.Row = Reihe
            FlexGrid2.Col = Spalte
            FlexGrid2.CellBackColor = &H80FFFF
        Next Spalte
    Next Reihe
     
'    For Spalte = 1 To 5
'        FlexGrid2.Row = 6
'        FlexGrid2.Col = Spalte
'        FlexGrid2.CellBackColor = &H80FFFF
'    Next Spalte
    
    For Reihe = 8 To 9
        For Spalte = 1 To 5
            FlexGrid2.Row = Reihe
            FlexGrid2.Col = Spalte
            FlexGrid2.CellBackColor = &H80FFFF
        Next Spalte
    Next Reihe
  
    For Reihe = 12 To 13
        For Spalte = 1 To 5
            FlexGrid2.Row = Reihe
            FlexGrid2.Col = Spalte
            FlexGrid2.CellBackColor = &H80FFFF
        Next Spalte
    Next Reihe
   
    For Spalte = 1 To 5
        FlexGrid2.Row = 18
        FlexGrid2.Col = Spalte
        FlexGrid2.CellBackColor = &H80FFFF
    Next Spalte
   
    
    'Grundwerte in Eingabefelder eintragen
    For Spalte = 1 To 5

        cboF1.ListIndex = 2
        FlexGrid2.TextMatrix(4, Spalte) = "CW" 'Betriebsart in Felxgrid Row 3 übernehmen
        FlexGrid2.TextMatrix(26, Spalte) = 0 'Listindex der Betriebsart in Felxgrid Row 26 übernehmen
        
        FlexGrid2.TextMatrix(0, Spalte) = 1.8
        FlexGrid2.TextMatrix(25, Spalte) = 0 'Frequenz ListIndex
        FlexGrid2.TextMatrix(26, Spalte) = 0 'Betriebsart Listindex
        FlexGrid2.TextMatrix(27, Spalte) = 18 'Frequenz ohne Komma 1.8 MHz Grundeintrag
        FlexGrid2.TextMatrix(22, Spalte) = 64.7 'Grenzwert für 1.8 MHz Grundeintrag
        FlexGrid2.TextMatrix(24, Spalte) = 0    'Gebäudedämpfung

        
        FlexGrid2.TextMatrix(2, Spalte) = "3"   'Abstand
        FlexGrid2.TextMatrix(3, Spalte) = "100"  'Leistung
        FlexGrid2.TextMatrix(5, Spalte) = "0.50"  'Aktivitätsfaktor
        FlexGrid2.TextMatrix(8, Spalte) = "0.00"    'Kabeldämpfung
        FlexGrid2.TextMatrix(9, Spalte) = "0.00"    'übrige Dämpfung
        FlexGrid2.TextMatrix(12, Spalte) = "0.00"   'Antennengewinn
        FlexGrid2.TextMatrix(13, Spalte) = "0.00"   'vertikale Winkeldämpfung
        FlexGrid2.TextMatrix(18, Spalte) = "0.00"   'Gebäudedämpfung
        FlexGrid2.TextMatrix(20, Spalte) = "1.60" 'Bodenreflexionsfaktor
    Next Spalte

        lbl06.Top = FlexGrid2.Top + FlexGrid2.Height - 280 '- lbl06.Height - 30




End Sub


Private Sub Berechnung_spezifizieren()
    
    'FlexGrid2 sezifizieren
    '=========================
    
    'Text zentrieren
    
    Dim Spalte As Integer
    Dim Reihe As Integer
    'Fettschrift FlexGrid2
    For Spalte = 1 To 5
        FlexGrid2.Row = 0
        FlexGrid2.Col = Spalte
        FlexGrid2.CellFontBold = True
    Next Spalte

    For Spalte = 1 To 5
        FlexGrid2.Row = 21
        FlexGrid2.Col = Spalte
        FlexGrid2.CellFontBold = True
    Next Spalte

    For Spalte = 1 To 5
        FlexGrid2.Row = 23
        FlexGrid2.Col = Spalte
        FlexGrid2.CellFontBold = True
    Next Spalte
    
    'Format der Kolonneneinträge auf zwei Dezimalstellen umstellen
    For Spalte = 1 To 5
    For Reihe = 1 To 23
        FlexGrid2.TextMatrix(Reihe, Spalte) = Format(FlexGrid2.TextMatrix(Reihe, Spalte), "0.00")
    Next Reihe
    Next Spalte
    
            
    
    
    'ComboBox spezifizieren Frequenz
    '===============================
    cboF1.BackColor = &H80FFFF
'    cboF1.Text = "MHz"

'    cboF1.AddItem (1.8), 1
'    cboF1.AddItem (3.5), 2
'    cboF1.AddItem (7), 3
'    cboF1.AddItem (10), 4
'    cboF1.AddItem (14), 5
'    cboF1.AddItem (18), 6
'    cboF1.AddItem (21), 7
'    cboF1.AddItem (24), 8
'    cboF1.AddItem (28), 9
'    cboF1.AddItem (50), 10
'    cboF1.AddItem (144), 11
'    cboF1.AddItem (432), 12
'    cboF1.AddItem (1240), 13
'    cboF1.AddItem (2400), 14

    
'    cboF1.ItemData(1) = 18 'Grenzwert =647
'    cboF1.ItemData(2) = 35 '465
'    cboF1.ItemData(3) = 70 '324
'    cboF1.ItemData(4) = 100 '280
 '   cboF1.ItemData(5) = 140 '280
'    cboF1.ItemData(6) = 180 '280
'    cboF1.ItemData(7) = 210 '280
'    cboF1.ItemData(8) = 240 '280
'    cboF1.ItemData(9) = 280 '280
'    cboF1.ItemData(10) = 500 '280
'    cboF1.ItemData(11) = 1440 '280
'    cboF1.ItemData(12) = 4320 '286
'    cboF1.ItemData(13) = 12400 '485
'    cboF1.ItemData(14) = 24000 '610

'    cboF1.ListIndex = 2 'Erster Eintrag beim Start anzeigen
    
    'ComboBox spezifizieren Betriebsart
    '==================================
    
    cboM1.BackColor = &H80FFFF
    cboM1.AddItem ("CW"), 0
    cboM1.AddItem ("SSB"), 1
    cboM1.AddItem ("FM"), 2
    cboM1.AddItem ("RTTY"), 3
    cboM1.AddItem ("PSK31"), 4

    cboM1.ItemData(0) = 1
    cboM1.ItemData(1) = 2
    cboM1.ItemData(2) = 3
    cboM1.ItemData(3) = 4
    cboM1.ItemData(4) = 5
    
    cboM1.ListIndex = 0  'Erster Eintrag beim Start anzeigen
    cboM1.ToolTipText = LoadResString(1281 + RS)
    
    Combo_laden  '(Sub)CboG1 Laden
        
End Sub
    
    
Private Sub Textladen_Berechnung()
    
    'FlexGrid1 spezifizieren
    '===================

    'Flexfelder Text eintragen

    FlexGrid1.TextMatrix(0, 1) = LoadResString(1209 + RS)  'fett
    FlexGrid1.TextMatrix(1, 1) = LoadResString(1709 + RS)
    
    FlexGrid1.TextMatrix(2, 1) = LoadResString(1213 + RS)
    FlexGrid1.TextMatrix(3, 1) = LoadResString(1217 + RS)
    FlexGrid1.TextMatrix(4, 1) = LoadResString(1220 + RS)
    FlexGrid1.TextMatrix(5, 1) = LoadResString(1221 + RS)
    FlexGrid1.TextMatrix(6, 1) = LoadResString(1223 + RS)
    FlexGrid1.TextMatrix(7, 1) = LoadResString(1225 + RS)
    FlexGrid1.TextMatrix(8, 1) = LoadResString(1228 + RS)
    FlexGrid1.TextMatrix(9, 1) = LoadResString(1231 + RS)
    FlexGrid1.TextMatrix(10, 1) = LoadResString(1234 + RS)
    FlexGrid1.TextMatrix(11, 1) = LoadResString(1237 + RS)
    FlexGrid1.TextMatrix(12, 1) = LoadResString(1239 + RS)
    FlexGrid1.TextMatrix(13, 1) = LoadResString(1242 + RS)
    FlexGrid1.TextMatrix(14, 1) = LoadResString(1245 + RS)
    FlexGrid1.TextMatrix(15, 1) = LoadResString(1248 + RS)
    FlexGrid1.TextMatrix(16, 1) = LoadResString(1250 + RS)
    FlexGrid1.TextMatrix(17, 1) = LoadResString(1253 + RS)
    FlexGrid1.TextMatrix(18, 1) = LoadResString(1256 + RS)
    FlexGrid1.TextMatrix(19, 1) = LoadResString(1259 + RS)
'    FlexGrid1.TextMatrix(19, 1) = LoadResString(1261 + RS)
    FlexGrid1.TextMatrix(20, 1) = LoadResString(1264 + RS)
    FlexGrid1.TextMatrix(21, 1) = LoadResString(1266 + RS) 'fett
    FlexGrid1.TextMatrix(22, 1) = LoadResString(1270 + RS)
    FlexGrid1.TextMatrix(23, 1) = LoadResString(1273 + RS) 'fett
    
    
    
    'Text Spalte 2 eintragen
    FlexGrid1.TextMatrix(0, 2) = LoadResString(1210 + RS) 'fett
    FlexGrid1.TextMatrix(1, 2) = ""
    
    FlexGrid1.TextMatrix(2, 2) = LoadResString(1214 + RS)
    FlexGrid1.TextMatrix(3, 2) = LoadResString(1218 + RS)
    FlexGrid1.TextMatrix(5, 2) = LoadResString(1222 + RS)
    FlexGrid1.TextMatrix(6, 2) = LoadResString(1224 + RS)
    FlexGrid1.TextMatrix(7, 2) = LoadResString(1226 + RS)
    FlexGrid1.TextMatrix(8, 2) = LoadResString(1229 + RS)
    FlexGrid1.TextMatrix(9, 2) = LoadResString(1232 + RS)
    FlexGrid1.TextMatrix(10, 2) = LoadResString(1235 + RS)
    FlexGrid1.TextMatrix(11, 2) = LoadResString(1238 + RS)
    FlexGrid1.TextMatrix(12, 2) = LoadResString(1240 + RS)
    FlexGrid1.TextMatrix(13, 2) = LoadResString(1243 + RS)
    FlexGrid1.TextMatrix(14, 2) = LoadResString(1246 + RS)
    FlexGrid1.TextMatrix(15, 2) = LoadResString(1249 + RS)
    FlexGrid1.TextMatrix(16, 2) = LoadResString(1251 + RS)
    FlexGrid1.TextMatrix(17, 2) = LoadResString(1254 + RS)
    FlexGrid1.TextMatrix(18, 2) = LoadResString(1257 + RS)
    FlexGrid1.TextMatrix(19, 2) = LoadResString(1260 + RS)
'    FlexGrid1.TextMatrix(19, 2) = LoadResString(1262 + RS)
    FlexGrid1.TextMatrix(20, 2) = LoadResString(1265 + RS)
    FlexGrid1.TextMatrix(21, 2) = LoadResString(1267 + RS) 'fett
    FlexGrid1.TextMatrix(22, 2) = LoadResString(1271 + RS)
    FlexGrid1.TextMatrix(23, 2) = LoadResString(1274 + RS) 'fett
    
    'Text Spalte 4 eintragen
    FlexGrid1.TextMatrix(0, 3) = LoadResString(1211) ' + RS) 'fett
    FlexGrid1.TextMatrix(2, 3) = LoadResString(1215) ' + RS)
    FlexGrid1.TextMatrix(3, 3) = LoadResString(1219) ' + RS)
    FlexGrid1.TextMatrix(4, 3) = "[ ]"
    FlexGrid1.TextMatrix(5, 3) = "[ ]"
    FlexGrid1.TextMatrix(6, 3) = "[ ]"
    FlexGrid1.TextMatrix(7, 3) = LoadResString(1227) ' + RS)
    FlexGrid1.TextMatrix(8, 3) = LoadResString(1230) ' + RS)
    FlexGrid1.TextMatrix(9, 3) = LoadResString(1233) ' + RS)
    FlexGrid1.TextMatrix(10, 3) = LoadResString(1236) ' + RS)
    FlexGrid1.TextMatrix(11, 3) = "[ ]"
    FlexGrid1.TextMatrix(12, 3) = LoadResString(1241) ' + RS)
    FlexGrid1.TextMatrix(13, 3) = LoadResString(1244) ' + RS)
    FlexGrid1.TextMatrix(14, 3) = LoadResString(1247) ' + RS)
    FlexGrid1.TextMatrix(15, 3) = "[ ]"
    FlexGrid1.TextMatrix(16, 3) = LoadResString(1252) ' + RS)
    FlexGrid1.TextMatrix(17, 3) = LoadResString(1255) ' + RS)
    FlexGrid1.TextMatrix(18, 3) = LoadResString(1258) ' + RS)
    FlexGrid1.TextMatrix(19, 3) = "[ ]"
'    FlexGrid1.TextMatrix(19, 3) = LoadResString(1263) ' + RS)
    FlexGrid1.TextMatrix(20, 3) = "[ ]"
    FlexGrid1.TextMatrix(21, 3) = LoadResString(1268) ' + RS) 'fett
    FlexGrid1.TextMatrix(22, 3) = LoadResString(1272) ' + RS)
    FlexGrid1.TextMatrix(23, 3) = LoadResString(1275) ' + RS) 'fett
  
    'Fettschrift FlexGrid1
    Dim Spalte As Integer
    For Spalte = 1 To 4
        FlexGrid1.Row = 0
        FlexGrid1.Col = Spalte
        FlexGrid1.CellFontBold = True
    Next Spalte
  
    For Spalte = 1 To 4
        FlexGrid1.Row = 1
        FlexGrid1.Col = Spalte
        FlexGrid1.CellFontBold = True
    Next Spalte
  
    For Spalte = 1 To 4
        FlexGrid1.Row = 2
        FlexGrid1.Col = Spalte
        FlexGrid1.CellFontBold = True
    Next Spalte
  
    For Spalte = 1 To 4
        FlexGrid1.Row = 3
        FlexGrid1.Col = Spalte
        FlexGrid1.CellFontBold = True
    Next Spalte
  
    For Spalte = 1 To 4
        FlexGrid1.Row = 7
        FlexGrid1.Col = Spalte
        FlexGrid1.CellFontBold = True
    Next Spalte
  
    For Spalte = 1 To 4
        FlexGrid1.Row = 17
        FlexGrid1.Col = Spalte
        FlexGrid1.CellFontBold = True
    Next Spalte
  
  
     For Spalte = 1 To 4
        FlexGrid1.Row = 21
        FlexGrid1.Col = Spalte
        FlexGrid1.CellFontBold = True
    Next Spalte
 
    For Spalte = 1 To 4
        FlexGrid1.Row = 23
        FlexGrid1.Col = Spalte
        FlexGrid1.CellFontBold = True
    Next Spalte
 
  
    'auf erst Kolonnen für die Anzeige setzen
    lblB1.Caption = LoadResString(1307 + RS) & " 1"
    
    

End Sub


'========================
Private Sub FlexGrid2_Click()

    If FlexGrid2.Row = 0 Then
        FlexGrid2.Row = 1
    ElseIf FlexGrid2.Row = 6 Then
        FlexGrid2.Row = 8
    ElseIf FlexGrid2.Row = 7 Then
        FlexGrid2.Row = 8
    ElseIf FlexGrid2.Row = 10 Then
        FlexGrid2.Row = 12
    ElseIf FlexGrid2.Row = 11 Then
        FlexGrid2.Row = 12
    ElseIf FlexGrid2.Row = 14 Then
        FlexGrid2.Row = 18
    ElseIf FlexGrid2.Row = 15 Then
        FlexGrid2.Row = 18
    ElseIf FlexGrid2.Row = 16 Then
        FlexGrid2.Row = 18
    ElseIf FlexGrid2.Row = 17 Then
        FlexGrid2.Row = 18
    ElseIf FlexGrid2.Row = 19 Then
        FlexGrid2.Row = 0
    ElseIf FlexGrid2.Row = 20 Then
        FlexGrid2.Row = 0
    ElseIf FlexGrid2.Row = 21 Then
        FlexGrid2.Row = 0
    ElseIf FlexGrid2.Row = 22 Then
        FlexGrid2.Row = 0
    ElseIf FlexGrid2.Row = 23 Then
        FlexGrid2.Row = 0
    ElseIf FlexGrid2.Row = 24 Then
        FlexGrid2.Row = 0
'    ElseIf FlexGrid2.Row = 23 Then
'        FlexGrid2.Row = 0
'    Else
    End If
    
    'Felder von FlexGrid2 editieren
    
    'Aufruf des Klassen moduls
    Set clsFGEdit = New clsFlexGridEdit
    Set clsFGEdit.FlexGridControl = FlexGrid2

    Text1.Text = FlexGrid2.TextMatrix(1, 2)
    
End Sub


Private Sub cboM1_click()
    
    'Modulationsfaktor
    Dim X As Integer 'Listindex
    Dim Y As String  'Betriebsart
    X = cboM1.ItemData(cboM1.ListIndex)
    
    If X = 1 Then
    
        Y = "CW"
        mZ = 4  'Modulationsfaktor durch 10 teilen
    ElseIf X = 2 Then
        Y = "SSB"
        mZ = 2  'Modulationsfaktor durch 10 teilen
    ElseIf X = 3 Then
        Y = "FM"
        mZ = 10  'Modulationsfaktor durch 10 teilen
    ElseIf X = 4 Then
        Y = "RTTY"
        mZ = 10  'Modulationsfaktor durch 10 teilen
    ElseIf X = 5 Then
        Y = "PSK31"
        mZ = 10  'Modulationsfaktor durch 10 teilen
    
    End If
            
    FlexGrid2.TextMatrix(4, C1) = Y 'Betriebsart in Felxgrid Row 4 (3) übernehmen
    FlexGrid2.TextMatrix(26, C1) = cboM1.ListIndex 'Listindex der Betriebsart in Felxgrid Row 26 übernehmen
    
    'Rote Berechnungsanzeige
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub cboF1_Click()
    
    Frequenzwahl 'Sub

End Sub

Private Sub Frequenzwahl()

    'Sub Frequenzwahl -> Beim Berechnen oder beim ändern der Frequenz
    
    FlexGrid2.TextMatrix(0, C1) = cboF1.ItemData(cboF1.ListIndex) / 10  'Frequenz
    FlexGrid2.TextMatrix(25, C1) = cboF1.ListIndex 'Listenindex der Frequenz
    FlexGrid2.TextMatrix(27, C1) = cboF1.ItemData(cboF1.ListIndex) 'Frequenz ohne Komma
    
    'Feldstärke-Grenzwert Entsprechend der gewählten Frequenz in Flexgrid Row 22 eintragen
    If FlexGrid2.TextMatrix(25, C1) = 0 Then
        FlexGrid2.TextMatrix(22, C1) = 61.5
    ElseIf FlexGrid2.TextMatrix(25, C1) = 1 Then
        FlexGrid2.TextMatrix(22, C1) = 44.6
    ElseIf FlexGrid2.TextMatrix(25, C1) = 2 Then
        FlexGrid2.TextMatrix(22, C1) = 32.4
    ElseIf FlexGrid2.TextMatrix(25, C1) = 3 Then
        FlexGrid2.TextMatrix(22, C1) = 28
    ElseIf FlexGrid2.TextMatrix(25, C1) = 4 Then
        FlexGrid2.TextMatrix(22, C1) = 28
    ElseIf FlexGrid2.TextMatrix(25, C1) = 5 Then
        FlexGrid2.TextMatrix(22, C1) = 28
    ElseIf FlexGrid2.TextMatrix(25, C1) = 6 Then
        FlexGrid2.TextMatrix(22, C1) = 28
    ElseIf FlexGrid2.TextMatrix(25, C1) = 7 Then
        FlexGrid2.TextMatrix(22, C1) = 28
    ElseIf FlexGrid2.TextMatrix(25, C1) = 8 Then
        FlexGrid2.TextMatrix(22, C1) = 28
    ElseIf FlexGrid2.TextMatrix(25, C1) = 9 Then
        FlexGrid2.TextMatrix(22, C1) = 28
    ElseIf FlexGrid2.TextMatrix(25, C1) = 10 Then
        FlexGrid2.TextMatrix(22, C1) = 28
    ElseIf FlexGrid2.TextMatrix(25, C1) = 11 Then
        FlexGrid2.TextMatrix(22, C1) = 28.5
    ElseIf FlexGrid2.TextMatrix(25, C1) = 12 Then
        FlexGrid2.TextMatrix(22, C1) = 48.4
    ElseIf FlexGrid2.TextMatrix(25, C1) = 13 Then
        FlexGrid2.TextMatrix(22, C1) = 61
    ElseIf FlexGrid2.TextMatrix(25, C1) = 14 Then
        FlexGrid2.TextMatrix(22, C1) = 61
    ElseIf FlexGrid2.TextMatrix(25, C1) = 15 Then
        FlexGrid2.TextMatrix(22, C1) = 61
    End If
    
    'Format des Kolonneneintrages auf zwei Dezimalstellen umstellen
    FlexGrid2.TextMatrix(22, C1) = Format(FlexGrid2.TextMatrix(22, C1), "0.00")
    
    
    'Auslesen der Frequenz und in Antennenfeld übertragen
    lbl14.Caption = cboF1.ListIndex
    cboA1.ListIndex = lbl14.Caption
    cboK51.ListIndex = lbl14.Caption
    
    'Rote Berechnungsanzeige
    lbl76.Caption = Int(Rnd * 100) + 2
    
End Sub



Private Sub FlexGrid2_Scroll()

    Kolonneverschieben
    
    
End Sub


Private Sub Kolonneverschieben()
    
    cboA1_sperren = 1  'Sperrt Antennen bearbeitung
    'Anzeige der aktuellen Kolonne
    lblB1.Caption = LoadResString(1307 + RS) & " " & FlexGrid2.LeftCol
    
    C1 = FlexGrid2.LeftCol
    cboF1.ListIndex = FlexGrid2.TextMatrix(25, C1) 'Frequenz. Listenindex auf Flexgrid-Eintrag aus Row 25 setzen
    cboF1.ItemData(cboF1.ListIndex) = FlexGrid2.TextMatrix(27, C1) 'Frequenz auf Flexgrid-Eintrag aus Row 27 setzen
    cboM1.ListIndex = FlexGrid2.TextMatrix(26, C1) 'Betriebsart auf Flexgrid-Eintrag aus Row 26 setzen
    
    FlexGrid2.TextMatrix(25, C1) = cboF1.ListIndex 'Listindex der Frequenz in Flexgrid Row 25 eintragen
    FlexGrid2.TextMatrix(27, C1) = cboF1.ItemData(cboF1.ListIndex) ' Frequenz in Flexgrid Row 27 eintragen
    FlexGrid2.TextMatrix(26, C1) = cboM1.ListIndex ' Listenindex der Betriebsart in Flexgrid Row 26 eintragen


'?????????????????????????????????????????????????
'==================================================

    'Neu 14.01.2004
    'Eintrag des ComBox Indexes in das Flexgrid
    'sofern ein Eintrag vorhanden
    If FlexGrid2.TextMatrix(24, C1) = " " Then      'Wenn Text fehlt
        cboG1.ListIndex = 0
        FlexGrid2.TextMatrix(24, C1) = 0
    Else
        cboG1.ListIndex = FlexGrid2.TextMatrix(24, C1)
        FlexGrid2.TextMatrix(24, C1) = cboG1.ListIndex  'Listindex von Gebäudedämpfung in FlexGrid eintragen
    End If
'    If cboG1.ListIndex = -1 Then
'        FlexGrid2.TextMatrix(24, C1) = 0
'    ElseIf FlexGrid2.TextMatrix(24, C1) = " " Then
'        cboG1.ListIndex = 0
'    Else
'        cboG1.ListIndex = FlexGrid2.TextMatrix(24, C1)
'    End If

'===============================================
'??????????????????????????????????????????????


    
    'Im Ausdruckformular aktive Kolonne mit gelber Farbe füllen
    'Kolonne weiss füllen wenn nicht mehr aktiv
    
    Dim Reihe As Integer
    Dim Spalte As Integer
    
    fGrid1.Flex1.Redraw = False 'FlexGrid sperren
    
    For Reihe = 0 To 23
         Spalte = FlexGrid2.LeftCol
        
        'Wenn Kolonne 1 gelb dann Kolonne 2 und 5 weiss füllen
        If FlexGrid2.LeftCol = 1 Then
            fGrid1.Flex1.Col = FlexGrid2.LeftCol + 7 '6
            fGrid1.Flex1.Row = Reihe
            fGrid1.Flex1.CellBackColor = &H80000005 'Weiss
            fGrid1.Flex1.Col = FlexGrid2.LeftCol + 4
            fGrid1.Flex1.Row = Reihe
            fGrid1.Flex1.CellBackColor = &H80000005 'Weiss
           
            
            fGrid1.Flex1.Col = FlexGrid2.LeftCol + 3 '2
            fGrid1.Flex1.CellBackColor = &H80FFFF 'Gelb
        End If
        
        'Kolonnne 2 - 4
        If Spalte > 1 And Spalte < 5 Then
            'Kolonne gelb
            fGrid1.Flex1.Col = FlexGrid2.LeftCol + 3 '2
            fGrid1.Flex1.Row = Reihe
            fGrid1.Flex1.CellBackColor = &H80FFFF
            'Kolonne -1 weiss
            fGrid1.Flex1.Col = FlexGrid2.LeftCol + 2 '1
            fGrid1.Flex1.CellBackColor = &H80000005
            'Kolonne +1 weiss
            fGrid1.Flex1.Col = FlexGrid2.LeftCol + 4
            fGrid1.Flex1.CellBackColor = &H80000005
        
        End If
        
        'Kolonne 5
        If Spalte = 5 Then
            'Kolonne gelb
            fGrid1.Flex1.Col = FlexGrid2.LeftCol + 3
            fGrid1.Flex1.Row = Reihe
            fGrid1.Flex1.CellBackColor = &H80FFFF
            'Kolonne -1 weiss
            fGrid1.Flex1.Col = FlexGrid2.LeftCol + 2
            fGrid1.Flex1.CellBackColor = &H80000005
        End If
        
    Next Reihe
    
    fGrid1.Flex1.Redraw = True  'FlexGrid freigeben
    
    lbl225.Caption = FlexGrid2.TextMatrix(2, Spalte) 'Abstand eintragen

    cboA1_sperren = 0  'Freigabe Antennen bearbeitung

'neu 5.4.08
cboA1_Click
    
End Sub




Private Sub cmd03_Click()

    NIS_berechnen
    
End Sub


Private Sub NIS_berechnen()

Dim mitl_Feldstärke As Double
'Berechnen
'=========
    Frequenzwahl 'Sub Einfügen des Feldstärkegrenzwertes
    
    FlexGrid2.TextMatrix(20, C1) = 1.6 'Einfügen des Bodenreflexionsfaktors
    
    'Berechnung der Tabellewerte frmMain
     On Error GoTo Textfehlt_Err
    
'Fehlerbehandlung
Textfehlt_Err:                            Select Case Err.Number
    
    Case 13
    
    'Fehlermeldung
    MsgBox Error, 16, "Ein Eingabefeld nicht oder falsch ausgefüllt"
    
    'Korrekturen in den Eingabefeldern
    If FlexGrid2.TextMatrix(2, C1) = "." Or FlexGrid2.TextMatrix(2, C1) = Ascii Or FlexGrid2.TextMatrix(2, C1) = " " Then
        FlexGrid2.TextMatrix(2, C1) = 10
    End If
    
    If FlexGrid2.TextMatrix(3, C1) = "." Or FlexGrid2.TextMatrix(3, C1) = Ascii Or FlexGrid2.TextMatrix(3, C1) = " " Then
        FlexGrid2.TextMatrix(3, C1) = 100
    End If
    
    If FlexGrid2.TextMatrix(5, C1) = "." Or FlexGrid2.TextMatrix(5, C1) = Ascii Or FlexGrid2.TextMatrix(5, C1) = " " Then
        FlexGrid2.TextMatrix(5, C1) = 100
    End If
    
    If FlexGrid2.TextMatrix(8, C1) = "." Or FlexGrid2.TextMatrix(8, C1) = Ascii Or FlexGrid2.TextMatrix(8, C1) = " " Then
        FlexGrid2.TextMatrix(8, C1) = 0
    End If
    
    If FlexGrid2.TextMatrix(9, C1) = "." Or FlexGrid2.TextMatrix(9, C1) = Ascii Or FlexGrid2.TextMatrix(9, C1) = " " Then
        FlexGrid2.TextMatrix(9, C1) = 0
    End If
    
    If FlexGrid2.TextMatrix(12, C1) = "." Or FlexGrid2.TextMatrix(12, C1) = Ascii Or FlexGrid2.TextMatrix(12, C1) = " " Then
        FlexGrid2.TextMatrix(12, C1) = 0
    End If
    
    If FlexGrid2.TextMatrix(13, C1) = "." Or FlexGrid2.TextMatrix(13, C1) = Ascii Or FlexGrid2.TextMatrix(13, C1) = " " Then
        FlexGrid2.TextMatrix(13, C1) = 0
    End If
    
    If FlexGrid2.TextMatrix(18, C1) = "." Or FlexGrid2.TextMatrix(18, C1) = Ascii Or FlexGrid2.TextMatrix(18, C1) = " " Then
        FlexGrid2.TextMatrix(18, C1) = 0
    End If
    
    End Select
    
    'Horizontal drehbar, Text Grad unterdrücken wenn nicht drehbar
    If txtA8.Text = LoadResString(1757 + RS) Then    '"Nein" Then
        txtA10.Text = ""
    End If
    
    'Vertikal drehbar, Text Grad unterdrücken wenn nicht drehbar
    If txtA9.Text = LoadResString(1757 + RS) Then    '"Nein" Then
        txtA11.Text = ""
    End If


    'Berechnungsformeln
    '==================
'    On Error Resume Next
    
'    FlexGrid2.TextMatrix(5, C1) = mZ / 10
    FlexGrid2.TextMatrix(6, C1) = mZ / 10
'    FlexGrid2.TextMatrix(6, C1) = Round((FlexGrid2.TextMatrix(4, C1) * FlexGrid2.TextMatrix(5, C1) * FlexGrid2.TextMatrix(2, C1)), 2)
    FlexGrid2.TextMatrix(7, C1) = Round((FlexGrid2.TextMatrix(5, C1) * FlexGrid2.TextMatrix(6, C1) * FlexGrid2.TextMatrix(3, C1)), 2)
    
'FG Row +1
    Dim Zahl1 As Double
    Zahl1 = CDbl(FlexGrid2.TextMatrix(8, C1))
    Dim Zahl2 As Double
    Zahl2 = CDbl(FlexGrid2.TextMatrix(9, C1))
    FlexGrid2.TextMatrix(10, C1) = Round(Zahl1 + Zahl2, 2)
    
'    FlexGrid2.TextMatrix(11, C1) = Round(10 ^ (-FlexGrid2.TextMatrix(10, C1) / 10), 3)
    FlexGrid2.TextMatrix(11, C1) = Round(10 ^ (-FlexGrid2.TextMatrix(10, C1) / 10), 6)
    
    Dim Zahl3 As Double
    Zahl3 = CDbl(FlexGrid2.TextMatrix(12, C1))
    Dim Zahl4 As Double
    Zahl4 = CDbl(FlexGrid2.TextMatrix(13, C1))
'    FlexGrid2.TextMatrix(14, C1) = Round(Zahl3 - Zahl4, 2)
    FlexGrid2.TextMatrix(14, C1) = Round(Zahl3 - Zahl4, 6)

'    FlexGrid2.TextMatrix(15, C1) = Round(10 ^ (FlexGrid2.TextMatrix(14, C1) / 10), 2)
    FlexGrid2.TextMatrix(15, C1) = Round(10 ^ (FlexGrid2.TextMatrix(14, C1) / 10), 6)
    
'    FlexGrid2.TextMatrix(16, C1) = Round(FlexGrid2.TextMatrix(7, C1) * FlexGrid2.TextMatrix(11, C1) * FlexGrid2.TextMatrix(15, C1), 2)
    FlexGrid2.TextMatrix(16, C1) = Round(FlexGrid2.TextMatrix(7, C1) * FlexGrid2.TextMatrix(11, C1) * FlexGrid2.TextMatrix(15, C1), 6)
    
'    FlexGrid2.TextMatrix(17, C1) = Round(FlexGrid2.TextMatrix(16, C1) / 1.64, 2)
    FlexGrid2.TextMatrix(17, C1) = Round(FlexGrid2.TextMatrix(16, C1) / 1.64, 6)
    
'    FlexGrid2.TextMatrix(19, C1) = Round(10 ^ (-FlexGrid2.TextMatrix(18, C1) / 10), 2) 'Gebäudedämpfungsfaktor
    FlexGrid2.TextMatrix(19, C1) = Round(10 ^ (-FlexGrid2.TextMatrix(18, C1) / 10), 6)
'00
'    FlexGrid2.TextMatrix(19, C1) = Round((Sqr(30 * FlexGrid2.TextMatrix(15, C1) * FlexGrid2.TextMatrix(18, C1))) / FlexGrid2.TextMatrix(1, C1), 2)
'    mitl_Feldstärke = Round((Sqr(30 * FlexGrid2.TextMatrix(16, C1) * FlexGrid2.TextMatrix(19, C1))) / FlexGrid2.TextMatrix(2, C1), 2)
    mitl_Feldstärke = Round((Sqr(30 * FlexGrid2.TextMatrix(16, C1) * FlexGrid2.TextMatrix(19, C1))) / FlexGrid2.TextMatrix(2, C1), 6)
    
''    FlexGrid2.TextMatrix(21, C1) = Round((FlexGrid2.TextMatrix(20, C1) * (Sqr(30 * FlexGrid2.TextMatrix(15, C1) * FlexGrid2.TextMatrix(18, C1)))) / FlexGrid2.TextMatrix(1, C1), 2)
'    FlexGrid2.TextMatrix(21, C1) = Round((FlexGrid2.TextMatrix(20, C1) * (Sqr(30 * FlexGrid2.TextMatrix(16, C1) * FlexGrid2.TextMatrix(19, C1)))) / FlexGrid2.TextMatrix(2, C1), 2)
    FlexGrid2.TextMatrix(21, C1) = Round((FlexGrid2.TextMatrix(20, C1) * (Sqr(30 * FlexGrid2.TextMatrix(16, C1) * FlexGrid2.TextMatrix(19, C1)))) / FlexGrid2.TextMatrix(2, C1), 6)
    
''    FlexGrid2.TextMatrix(23, C1) = Round((FlexGrid2.TextMatrix(20, C1) * (Sqr(30 * FlexGrid2.TextMatrix(15, C1) * FlexGrid2.TextMatrix(18, C1)))) / FlexGrid2.TextMatrix(22, C1), 2)
'    FlexGrid2.TextMatrix(23, C1) = Round((FlexGrid2.TextMatrix(20, C1) * (Sqr(30 * FlexGrid2.TextMatrix(16, C1) * FlexGrid2.TextMatrix(19, C1)))) / FlexGrid2.TextMatrix(22, C1), 2)
    FlexGrid2.TextMatrix(23, C1) = Round((FlexGrid2.TextMatrix(20, C1) * (Sqr(30 * FlexGrid2.TextMatrix(16, C1) * FlexGrid2.TextMatrix(19, C1)))) / FlexGrid2.TextMatrix(22, C1), 6)
    
    'Kopieren der berechnenten Werte in Form 2
    'Kopieren der Texte aus Flex 1 in Form 2
    
    Dim Reihe As Integer
    Dim Spalte As Integer
    
    'Format der Kolonneneinträge auf zwei Dezimalstellen umstellen
    For Reihe = 3 To 23
        FlexGrid2.TextMatrix(Reihe, C1) = Format(FlexGrid2.TextMatrix(Reihe, C1), "0.00")
    Next Reihe
    
    'Berechnete Werte in Spalten 4 - 8 eintragen
'    For Reihe = 0 To 2
    For Reihe = 0 To 3
         Spalte = C1
            fGrid1.Flex1.TextMatrix(Reihe, Spalte + 3) = FlexGrid2.TextMatrix(Reihe, Spalte)
    Next Reihe
    
'    For Reihe = 4 To 27
    For Reihe = 5 To 27
         Spalte = C1
            fGrid1.Flex1.TextMatrix(Reihe - 1, Spalte + 3) = FlexGrid2.TextMatrix(Reihe, Spalte)
    Next Reihe
    
    
    
    'Text der ersten 3 Spalten eintragen
    For Reihe = 0 To 2
        For Spalte = 1 To 3
'            fGrid1.Flex1.TextMatrix(Reihe, Spalte) = FlexGrid1.TextMatrix(Reihe, Spalte)
        Next Spalte
    Next Reihe

    For Reihe = 4 To 23
        For Spalte = 1 To 3
'            fGrid1.Flex1.TextMatrix(Reihe - 1, Spalte) = FlexGrid1.TextMatrix(Reihe, Spalte)
        Next Spalte
    Next Reihe

    
        For Spalte = 1 To 3
'            fGrid1.Flex1.TextMatrix(27, Spalte) = FlexGrid1.TextMatrix(3, Spalte)
        Next Spalte

    lbl225.Caption = FlexGrid2.TextMatrix(2, C1)

    'Zufallszahl wenn Taste Berechnen gedrückt wird

    fGrid1.lblGrid1.Caption = Int(Rnd * 100) + 2
    
    'Antennentyp in Druckerformular übernehmen
    fGrid1.MSFlex0.TextMatrix(2, 2) = "Antenne: " & txtA19.Text
    
    'Button Farbe Berechnung im Originalzustand
    Stay = 0
    blk = 0
    cmd03.BackColor = &HC0C0C0
    
    'Gespeicherter Antennentyp zurückkopieren
    fGrid1.MSFlex0.TextMatrix(5, 2) = flexA1.TextMatrix(1, 2)


End Sub

'
'================================================================================
'
'Verhinderung der Fehleingaben ausser Zahlen und
'Punkt, Komma wird in Punkt umgewandelt
'
'
'
'------------------------------
'  Koaxialkabel-Dämpfung
'------------------------------
'

'
Private Sub cboK31_Click()

   Kabel_berechnen

    lblK99.Caption = Int(Rnd * 100) + 2
    lblK1.Caption = cboK31.List(cboK31.ListIndex)
    
End Sub

Private Sub cboK131_Click()

    Kabel_berechnen

    lblK99.Caption = Int(Rnd * 100) + 2
    
End Sub

Private Sub cboK231_Click()

    Kabel_berechnen

    lblK99.Caption = Int(Rnd * 100) + 2
    
End Sub


'Generieren einer Zufallszahl wenn etwas geändert wird
'und speichern in label K99
'

Private Sub cboK51_Click()

    'Frequenzwahl
    lblK99.Caption = Int(Rnd * 100) + 2
    lblK52.Caption = cboK51.ListIndex
    
    Kabel_berechnen
   
    'Frequenz in andere Listen übertragen
    cboA1.ListIndex = lblK52.Caption
    cboF1.ListIndex = lblK52.Caption
    
End Sub




'_______________________________
'Antennen
'__________________________________


Private Sub txtA6_Change()
    
    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub


Private Sub txtA8_Change()
    
    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub

Private Sub txtA8_Click()

    SelectAll txtA8

End Sub

Private Sub txtA8_GotFocus()

    SelectAll txtA8
    
End Sub

Private Sub txtA9_Change()
    
    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub

Private Sub txtA9_Click()

    SelectAll txtA9

End Sub

Private Sub txtA9_GotFocus()

    SelectAll txtA9
    
End Sub

Private Sub txtA10_KeyPress(KeyAscii As Integer)

    Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txtA10.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub

Private Sub txtA10_Change()
    
    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub

Private Sub txtA10_Click()

    SelectAll txtA10

End Sub

Private Sub txtA10_GotFocus()

    SelectAll txtA10
    
End Sub

Private Sub txtA11_KeyPress(KeyAscii As Integer)
    
    Select Case KeyAscii
    Case Asc("0") To Asc("9")
    Case Asc(","), Asc(".")
    If InStr(txtA11.Text, ",") <> 0 Then
    KeyAscii = 0
    Else
    KeyAscii = Asc(".")
    End If
    Case Asc(vbBack)
    Case Else
    KeyAscii = 0
    End Select
End Sub


Private Sub txtA11_Change()
    
    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2

End Sub

Private Sub txtA11_Click()

    SelectAll txtA11

End Sub

Private Sub txtA11_GotFocus()

    SelectAll txtA11
    
End Sub



Private Sub cboA1_Click()

    If cboA1_sperren = 0 Then 'Sperrt Antennen bearbeitung

        'Auslesen der Frequenz und Index in LblA2 speichern
        lblA2.Caption = cboA1.ItemData(cboA1.ListIndex)
    
        'Gewählte Kolonne in lblA16 übertragen
        'flexA1.Col = lblA16.Caption
        lblA16.Caption = flexA1.Col

        'Frequenz übertragen in Formular Berechnung und Kabel übertragen
        cboF1.ListIndex = lblA2.Caption - 1
        cboK51.ListIndex = lblA2.Caption - 1

        'löschen von Inhalt txtA2 (Gewinn)
        txtA2.Text = 0

        'Eintrag des Antennengewinns in txtA2
 
        If flexA1.TextMatrix(lblA2.Caption, 4) = Ascii Then
         
            txtA2.Text = 0
        Else
            txtA2.Text = flexA1.TextMatrix(lblA2.Caption, 4)
        End If
    
        'Uebertragen des Gewinns auf Hauptformular
        'FlexGrid2.TextMatrix(11, C1) = txtA2.Text
        FlexGrid2.TextMatrix(12, C1) = txtA2.Text
        
        'Anzeigen der Winkeldämpfung sofern Spaltenwahl grösser 4
        If lblA16.Caption > 4 Then

            If flexA1.TextMatrix(lblA2.Caption, lblA16.Caption) = Ascii Then
                txtA6.Text = 0
            Else
 
                txtA6.Text = flexA1.TextMatrix(lblA2.Caption, lblA16.Caption)
            End If
    
            'Uebertragen der Winkeldämpfung auf Hauptformular
            'FlexGrid2.TextMatrix(12, C1) = txtA6.Text
            FlexGrid2.TextMatrix(13, C1) = txtA6.Text
            
            'Anzeigen des gewählten Winkels der Winkeldampfung
            txtA7.Text = flexA1.TextMatrix(0, lblA16.Caption)

        End If

    End If

End Sub

Private Sub flexA1_Click()

    'Frequenz einlesen
    lblA2.Caption = cboA1.ItemData(cboA1.ListIndex)

    'Anzeige der selektierten Zelle (Colone und Reihe)
    lblA15.Caption = flexA1.Row
    lblA16.Caption = flexA1.Col
    
    'Nur ausführen wenn im Feld Winkeldämpung 0-90° geklickt wird
    If lblA16.Caption > 4 Then
 
        'Eintrag des Antennengewinns in txtA2
 
        If flexA1.TextMatrix(lblA2.Caption, 4) = Ascii Then
         
            txtA2.Text = 0
         
        Else
            txtA2.Text = flexA1.TextMatrix(lblA2.Caption, 4)
         
        End If
    
        'Uebertragen des Gewinns auf Hauptformular
        'FlexGrid2.TextMatrix(11, C1) = txtA2.Text
        FlexGrid2.TextMatrix(12, C1) = txtA2.Text
 
 
        'Anzeigen der Winkeldämpfung
        If flexA1.TextMatrix(lblA2.Caption, lblA16.Caption) = Ascii Then
            txtA6.Text = 0
        Else
 
            txtA6.Text = flexA1.TextMatrix(lblA2.Caption, lblA16.Caption)
        End If
    
        'Uebertragen der Winkeldämpfung auf Hauptformular
        'FlexGrid2.TextMatrix(12, C1) = txtA6.Text
        FlexGrid2.TextMatrix(13, C1) = txtA6.Text
        'Anzeigen des gewählten Winkels der Winkeldampfung
        txtA7.Text = flexA1.TextMatrix(0, lblA16.Caption)
    
        'Übertragen der Winkeldämpfung auf Ausdruck (versteckt)
        fGrid1.MSFlex0.TextMatrix(5, 3) = lblA16.Caption

    End If
  
    'Winkeldämfungen in Diagramm übertragen
    Dim wd As Integer
  
    For wd = 1 To 10
        frmS1.flexS1.Row = wd
        frmS1.flexS1.Col = 2
        frmS1.flexS1.Text = flexA1.TextMatrix(lblA2.Caption, wd + 4)
    Next
    
End Sub


Private Sub lblA7_change()

'Flexgrid Antennendaten laden


On Error GoTo Dateifehler_Err
        
Dateifehler_Err:
    
    Label4.Caption = Err.Number
    If Label4.Caption > 0 Then
        Label4.BackColor = vbYellow
    End If
    
    Select Case Err.Number
    
    'Fehlermeldung
    Case 20
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
    
    Case 53
    MsgBox Error, 16, "Datei nicht gefunden,  " & LoadResString(1108 + RS)
    'MsgBox Error, 16, "Datei nicht gefunden (Antennendaten aufbereiten )"
    
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
   
    
    Case 16
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
    End Select
'========


    Dim Min As Integer
    'Fortschrittsanzeige
    ProgressBar1.Value = Min
    ProgressBar1.Visible = True
 
    txtA2.Text = 0
    txtA6.Text = 0
    txtA7.Text = 0
    FlexGrid2.TextMatrix(12, C1) = 0
    FlexGrid2.TextMatrix(13, C1) = 0


    'FlexA1 mit File von Disk laden
        
    'Flexgrid auf 16 Kolonnen setzen, spez für Files mit CR und LF Marke
    flexA1.Cols = 16
  
    TextBox1.Text = ""   ' Textbox leeren
    
    Dim Text1
    
    
    'lblT1=Pfadangabe. Datei zum Einlesen öffnen.
    Open lblT1.Caption For Input As #1

    Do While Not EOF(1)   ' Schleife bis Dateiende.
        Input #1, Text1 ' Daten in zwei Variablen einlesen.
        TextBox1.Text = TextBox1.Text + Text1 + vbCrLf
    Loop

    Close #1   ' Datei schließen
 
 On Error Resume Next
  ' Tabelle löschen
    flexA1.Clear
    
    flexA1.Redraw = False
        
    Dim X As Long  'Kolonne                                                      ' Variable für die Spalten
    Dim Y As Long  'Reihe                                                      ' Variable für die Zeilen
    Dim Start As Long                                                    'Variable für den Markierungsanfang
    Dim Ende As Long                                                     'Variable für das Markierungsende
    Ende = 1
   

    For Y = 0 To flexA1.Rows - 1                              'ermittelt die Anzahlt der Zeilen
        For X = 0 To flexA1.Cols - 1                                'ermittelt die Anzahlt der Spalten
            Start = InStr(Ende, TextBox1.Text, ":", 0)          ' Sucht nach dem Simikolon und gibt die Position in einer Zahl aus,
            TextBox1.SelStart = Start                                ' Markierungsanfang setzen
            Ende = InStr(Start + 1, TextBox1.Text, ":", 0)  'Markierungsende errechnen
            TextBox1.SelLength = Ende - 1 - Start                    ' Markierungsende setzen.
                    
            'Plazierung des Textes im Flexgrid
            flexA1.Col = X
            flexA1.Row = Y

            flexA1.Text = TextBox1.SelText
                        
            flexA1.Col = X
            
            'Fortschrittsanzeige erhöhen um 1
            ProgressBar1.Value = ProgressBar1.Value + 1
        
        Next X
    Next Y

'ErrHandler:
    If Not Err = cdlCancel Then Resume Next
        
'    flexA1.Redraw = True
        
    flexA1.Cols = 15   'Auf 15 Kolonnen zurücksetzen
    
    
    'Titelleiste fett schreiben
    Dim xc As Integer
    For xc = 1 To 14
    flexA1.Row = 0
    flexA1.Col = xc
    flexA1.CellFontBold = True
    Next
    
    flexA1.Redraw = True

    'Cursor im FlexGrid auf eine unsichtbare Position setzen
    flexA1.Col = 0
    flexA1.Row = 0

    
    txtA2.Text = 0
    txtA6.Text = 0
    txtA7.Text = 0
        

    'Frequenzauswahl einlesen
    lblA2.Caption = cboA1.ItemData(cboA1.ListIndex)
        
    'Antennengewinn oben anzeigen
    If flexA1.TextMatrix(lblA2.Caption, 4) = Ascii Then
         
         txtA2.Text = 0
    Else
         txtA2.Text = flexA1.TextMatrix(lblA2.Caption, 4)
    End If
    flexA1.TextMatrix(15, 3) = "5650"
    flexA1.TextMatrix(16, 3) = "10000"
    
    flexA1.Redraw = True
    
    ProgressBar1.Visible = False  'Fortschrittanzeige unterdrücken
    ProgressBar1.Value = Min      'Fortschrittanzeige auf min. setzen
  
    'Uebertragen des Gewinns auf Hauptformular
    FlexGrid2.TextMatrix(12, C1) = txtA2.Text
    


    'Text einblenden wenn keine Werte für Diagramm vorhanden
    lblA18.Caption = flexA1.TextMatrix(lblA2.Caption, 5)
  
      
    kein_Diagramm     'Subrutine
    
    flexA1.Col = 0    'auf 0 gesetzt damit ein Wert eingetragen
    
End Sub


'====================Hersteller und Antennentypen

Private Sub lblH14_Change()


'Hersteller laden

On Error GoTo Dateifehler_Err
        
Dateifehler_Err:
    
    Label4.Caption = Err.Number
    If Label4.Caption > 0 Then
        Label4.BackColor = vbYellow
    End If
    
    Select Case Err.Number
    
    'Fehlermeldung
    Case 20
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
    
    Case 53
    MsgBox Error, 16, "Datei nicht gefunden,  " & LoadResString(1108 + RS) '(Antennendaten aufbereiten)"
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
   
    
    Case 16
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
    End Select
'========


'On Error Resume Next
    
    flexH1.Cols = 4     'Flexgrid auf 4 Kolonnen setzen für CR LF Eintrag
       
  ' Tabelle löschen
    flexH1.Clear
                        
    TextBoxH1.Text = ""   ' Textbox leeren
    
    Dim Text1
    Open App.Path & "\" & "Ant_Dat" & "\" & "Hersteller" & ".sup" For Input As #1   ' Datei zum Einlesen öffnen.
    
    Do While Not EOF(1)   ' Schleife bis Dateiende.
        Input #1, Text1 ' Daten in zwei Variablen einlesen.
        TextBoxH1.Text = TextBoxH1.Text + Text1 + vbCrLf
    Loop

    Close #1   ' Datei schließen

    On Error Resume Next

    Dim längezeilenH As String
    
    ' ermittelt die ersten 4 Zeichen
    längezeilenH = Left(TextBoxH1.Text, 4)
    'nimmt daraus die letzen 2 Zeichen = Anzahl Zeilen
    längezeilenH = Right(längezeilenH, 2)
       
    'schreibt den markierten String als Zeilenanzahl in die
    'MSFlexGrid Eigenschaft Rows!
    flexH1.Rows = längezeilenH + 1

    'Sperrt das Neuzeichnen des FlexTabelle.
    flexH1.Redraw = False
    
    'ermittelt die Anzahlt der Zeilen und Spalten.
    'ermittelt die Strings, welche zwischen den Dopelpunkten(:) liegen,
    'und schreibt die Strings in jede einzelne Zelle.
    Dim XH As Long           ' Variable für die Spalten
    Dim StartH As Long       'Variable für den Markierungsanfang
    Dim EndeH As Long        'Variable für das Markierungsende
    
    EndeH = 1
    
    For YH = 0 To flexH1.Rows - 1                              'ermittelt die Anzahlt der Zeilen
        For XH = 0 To flexH1.Cols - 1                                'ermittelt die Anzahlt der Spalten
            StartH = InStr(EndeH, TextBoxH1.Text, ":", 0)          ' Sucht nach dem Simikolon und gibt die Position in einer Zahl aus,
            TextBoxH1.SelStart = StartH                                ' Markierungsanfang setzen
            EndeH = InStr(StartH + 1, TextBoxH1.Text, ":", 0)  'Markierungsende errechnen
            TextBoxH1.SelLength = EndeH - 1 - StartH                    ' Markierungsende setzen.
                    
            'ändert die aktuellen Zelle entsprechend der For-Next-Umläufe
            flexH1.Col = XH
            flexH1.Row = YH

            'schreibt den selektierten Text in die aktive Zelle
            flexH1.Text = TextBoxH1.SelText
            
            'ändert die aktuellen Zelle wieder entsprechend der For-Next-Umläufe,
            'um den Zustand vor der farbeänderung herzustellen:
            flexH1.Col = XH
        Next XH
    Next YH

ErrHandler:
   If Not Err = cdlCancel Then Resume Next
            
    'Zurücksetzen auf 3 Kolonnen
    flexH1.Cols = 3
    
    'Aktiviert das Neuzeichnen des FlexTabelle.
    flexH1.Redraw = True
   
    flexH1.Row = 0
    flexH1.Col = 2
    flexH1.CellFontBold = True
   
   
End Sub

Private Sub flexH1_Click()



'Hersteller auswählen
    flexA1.Clear
    txtA2.Text = 0
    txtA6.Text = 0
    txtA7.Text = 0
'    txt122.Text = 0
'    txt132.Text = 0


    'Flexgrid füllen
    Dim Y As Integer
    For Y = 1 To 14
        'Flexgrid Kolonne 3 mit Frequenz füllen
        flexA1.TextMatrix(Y, 3) = (cboA1.List(Y - 1))
   
    Next Y


    'Tool Tip Text FlexA1 einfügen
    flexA1.ToolTipText = "Zur Übernahme der Winkeldämpfung in der entsprechenden Winkelkolonne anklicken"

    'Text : Row 0, Col 1 - 14 einfügen
 
    If RS = 0 Then
        flexA1.TextMatrix(0, 1) = "Hersteller"
        flexA1.TextMatrix(0, 2) = "Typ"
    End If
    
    If RS = 1000 Then
        flexA1.TextMatrix(0, 1) = "Constructeur"
        flexA1.TextMatrix(0, 2) = "Type"
    End If
    
      If RS = 2000 Then
        flexA1.TextMatrix(0, 1) = "Constructore"
        flexA1.TextMatrix(0, 2) = "Typo"
    End If
  
    
    
    flexA1.TextMatrix(0, 3) = "f(MHz)"
    flexA1.TextMatrix(0, 4) = "dBi"
    flexA1.TextMatrix(0, 5) = "  0° "
    flexA1.TextMatrix(0, 6) = "  10° "
    flexA1.TextMatrix(0, 7) = "  20° "
    flexA1.TextMatrix(0, 8) = "  30° "
    flexA1.TextMatrix(0, 9) = "  40° "
    flexA1.TextMatrix(0, 10) = "  50° "
    flexA1.TextMatrix(0, 11) = "  60° "
    flexA1.TextMatrix(0, 12) = "  70° "
    flexA1.TextMatrix(0, 13) = "  80° "
    flexA1.TextMatrix(0, 14) = "  90° "
    flexA1.TextMatrix(15, 3) = "5650"
    flexA1.TextMatrix(16, 3) = "10000"

'    Dim Herst As String
 
    Herst = (flexH1.TextMatrix(flexH1.Row, 1)) & ("_") & (flexH1.TextMatrix(flexH1.Row, 2))
    lblH1.Caption = App.Path & "\" & "Ant_Dat" & "\" & Herst & ".typ"
    fGrid1.MSFlex0.TextMatrix(5, 1) = Herst
    lblH13.Caption = Int(Rnd * 100) + 2
    
    'Anzeige wenn kein Diagramm vorhanden
    lblA18.Caption = flexA1.TextMatrix(lblA2.Caption, 5)
    kein_Diagramm   'subrutine

'neu 5.4.08
cboA1_Click
    
End Sub

Private Sub lblH13_change()

'Antennentypen (Flex T1)vom Diskladen

On Error GoTo Dateifehler_Err
        
Dateifehler_Err:
    
    Label4.Caption = Err.Number
    If Label4.Caption > 0 Then
        Label4.BackColor = vbYellow
    End If
    
    Select Case Err.Number
    
    'Fehlermeldung
    Case 20
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
    
    Case 53
    MsgBox Error, 16, "Datei nicht gefunden, " & LoadResString(1108 + RS) '(Antennendaten aufbereiten)"
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
   
    
    Case 16
    MsgBox Error, 16, "Pfadeingabe falsch "
    Exit Sub
    End Select
'========


'On Error Resume Next

  flexT1.Cols = 4 'Kolonne um 1 erhöhen
  flexT1.Rows = 1 'Zeile auf 1 setzen
  TextBoxT1.Text = ""  'Textbox löschen
    
    Dim Text1
    Open (lblH1.Caption) For Input As #1 'Datei mit Pfad (lblH1) öffnen
    Do While Not EOF(1)   ' Schleife bis Dateiende.
        Input #1, Text1 ' Daten in zwei Variablen einlesen.
        TextBoxT1.Text = TextBoxT1.Text + Text1 + vbCrLf
    Loop

    Close #1   ' Datei schließen

On Error Resume Next
    
    'Tabelle löschen
    flexT1.Clear
                            
    Dim längezeilenT As String
    
    ' ermittelt die ersten 4 Zeichen
    längezeilenT = Left(TextBoxT1.Text, 4)
    'nimmt daraus die letzen 2 Zeichen = Anzahl Zeilen
    längezeilenT = Right(längezeilenT, 2)
    
    
    'schreibt den markierten String als Zeilenanzahl in die
    'MSFlexGrid Eigenschaft Rows!
    flexT1.Rows = längezeilenT + 1
       
    'Sperrt das Neuzeichnen des FlexTabelle.
    flexT1.Redraw = False
    
    'ermittelt die Anzahlt der Zeilen und Spalten.
    'ermittelt die Strings, welche zwischen den Pipe-Zeichen(|) liegen,
    'und schreibt die Strings in jede einzelne Zelle.
    Dim u As Long          ' Variable für die Spalten
    Dim V As Long          ' Variable für die Zeilen
    Dim Start As Long      'Variable für den Markierungsanfang
    Dim Ende As Long       'Variable für das Markierungsende
    Ende = 1

'    For V = 0 To flexT1.Rows - 1                              'ermittelt die Anzahlt der Zeilen
    
    For V = -1 To flexT1.Rows - 1                              'ermittelt die Anzahlt der Zeilen
        For u = 0 To flexT1.Cols - 1                                'ermittelt die Anzahlt der Spalten
            Start = InStr(Ende, TextBoxT1.Text, ":", 0)          ' Sucht nach dem Simikolon und gibt die Position in einer Zahl aus,
            TextBoxT1.SelStart = Start                                ' Markierungsanfang setzen
            Ende = InStr(Start + 1, TextBoxT1.Text, ":", 0)  'Markierungsende errechnen
            TextBoxT1.SelLength = Ende - 1 - Start                    ' Markierungsende setzen.
                    
            'ändert die aktuellen Zelle entsprechend der For-Next-Umläufe
            flexT1.Col = u
            flexT1.Row = V

            'schreibt den selektierten Text in die aktive Zelle
            flexT1.Text = TextBoxT1.SelText
            
            'ändert die aktuellen Zelle wieder entsprechend der For-Next-Umläufe,
            'um den Zustand vor der farbeänderung herzustellen:
            flexT1.Col = u
        Next u
    Next V

ErrHandler:
    If Not Err = cdlCancel Then Resume Next
            
    'Aktiviert das Neuzeichnen des FlexTabelle.
    flexT1.Redraw = True
    flexT1.Cols = 3
    
    flexT1.Row = 0
    flexT1.Col = 2
    flexT1.CellFontBold = True
    
    
End Sub

Private Sub flexT1_Click()
    
'Antennen Typ auswählen




    txtA2.Text = 0
    txtA6.Text = 0
    txtA7.Text = 0
    FlexGrid2.TextMatrix(12, C1) = 0
    FlexGrid2.TextMatrix(13, C1) = 0
    
    
 '==== laden der eigenen Antennen sofern Eigen gewählt
 
    If flexT1.TextMatrix(0, 2) = "Eigene" Then
        Ty = (flexT1.TextMatrix(0, 2) & "\" & flexT1.TextMatrix(flexT1.Row, 2))
        lblT1.Caption = App.Path & "\" & "Ant_Dat" & "\" & Ty & ".ant"
    Else
 
 '=====lsden der Standartantennen
 
    Ty = (flexT1.TextMatrix(flexT1.Row, 2))
    lblT1.Caption = App.Path & "\" & "Ant_Dat" & "\" & Ty & ".ant"
    
    End If
    
    lblA7.Caption = Int(Rnd * 100) + 2

End Sub
    

Private Sub txtA2_Change()

     If cboA1_sperren = 0 Then 'Sperrt Antennen bearbeitung

        'Antennenhersteller und Typ in Hauptformular eintragen
        Dim antHerst As String
    
        antHerst = flexA1.TextMatrix(1, 1)  'Hersteller auslesen
        
        If antHerst = "Allgemein" Then      'Wenn Hersteller allg. unterdrücken
            antHerst = ""
        Else                                'sonst eintragen
    
        End If
    
        If txtA2.Text <> 0 Then             'Wenn Antennengewinn <> 0
            txtA19.Text = antHerst + " " + flexA1.TextMatrix(1, 2)    'Eintrag in Hauptformular
            lbl124.Caption = txtA19.Text
        Else
            lbl124.Caption = ""
            'frmS1.lblS1.Caption = ""   'Im Diagrammfeld
    
        End If
    
    
        'Aendern der Farbe von cmd03
        lbl76.Caption = Int(Rnd * 100) + 2
    
        Unload frmS1  'Diagramm löschen
    
    End If

End Sub


Private Sub cmdA1_click()
 
    Antennendaten_bearbeiten
 
End Sub

Private Sub Antennendaten_bearbeiten()
 
'Text in LblA18 einblenden wenn für dies Frequenz keine Werte vorhanden
'Siehe auch lblA2_Change (auch Text einblenden wenn Frequenz gewechselt wird)
'Siehe auch lblA7_Change

    lblA18.Caption = flexA1.TextMatrix(lblA2.Caption, 5)
    
    If lblA18.Caption = Asci Then
    
        kein_Diagramm   'Sub rutine
    
    Else
        frmS1.Show    'Diagrammfeld anzeigen (Einblenden)

        'Uebertragen des Gewinns in Diagrammformular
        frmS1.lblS5.Caption = LoadResString(1506 + RS) & " " & frmMain.txtA2.Text & " dB"

        'Frequenz eintragen
        frmS1.lblS3.Caption = LoadResString(1502 + RS) & " " & flexA1.TextMatrix(lblA2.Caption, 3) & " MHz"
        
        'Ant Hersteller eintragen
        frmS1.lblS1.Caption = flexA1.TextMatrix(1, 1) + "  " + flexA1.TextMatrix(1, 2) 'In Diagramm

        'Winkeldämfungen in Diagramm übertragen
        Dim wd As Integer
  
        For wd = 1 To 10
            frmS1.flexS1.Row = wd
            frmS1.flexS1.Col = 2
            frmS1.flexS1.Text = flexA1.TextMatrix(lblA2.Caption, wd + 4)
        Next

        frmS1.lblS2.Caption = Int(Rnd * 100) + 2
        
        lblA18.Caption = ""     '0 Anzeige unterdrücken
        
    End If

End Sub


Private Sub lblA2_Change()

    'Text einblenden wenn keine Werte für Diagramm vorhanden
    lblA18.Caption = flexA1.TextMatrix(lblA2.Caption, 5)
    
    kein_Diagramm   'Subrutine
    

End Sub

Sub kein_Diagramm()

    'Text in entsprechender Sprache einblenden
    If lblA18.Caption = Asci Then
    
    No_Diagram = 1 'Kein Diagramm vorhanden für Antennendatenausdruck
        
        If RS = 0 Then
            lblA18.Caption = LoadResString(1519 + RS)  '"Kein vertikales Strahlungs-Diagramm für diese Frequenz vorhanden"
        End If
        
        If RS = 1000 Then
            lblA18.Caption = LoadResString(1519 + RS)   'französisch
        End If
        
        If RS = 2000 Then
            lblA18.Caption = LoadResString(1519 + RS)   'italienisch
        End If
        
 '       Unload frmS1

    Else
        
        'Text ausblenden wenn Werte für Diagramm vorhanden
        lblA18.Caption = ""
        
        No_Diagram = 0 'Diagramm vorhanden für Antennendatenausdruck

        
    End If


End Sub

'==============================0
'
'Kabelliste erweitern
'
'==============================0

Public Sub DoKabeldatenuebertragen()



'Kabeldaten aus Flexgrid in Matrix eintragen

    Dim X As Integer   'Colonne
    Dim Y As Integer   'Row

    For X = 1 To FlexKabel1.Cols - 1 '9
        For Y = 1 To FlexKabel1.Rows - 1 '14
            Kabel(Y - 1, X - 1) = FlexKabel1.TextMatrix(Y, X)
        Next Y
    Next X

    'Kabelauswahl
    lstK1.Clear     'Listbox Dämpfungen leeren
    lstK2.Clear     'Listbox Dämpfungen leeren
    cboK31.Clear    'cbo Kabeltyp leeren
    'Kabeltypen eintragen
    For Y = 1 To FlexKabel1.Cols - 1  '9
        cboK31.AddItem FlexKabel1.TextMatrix(0, Y) '"Aircom plus", 0
    Next Y

    cboK31.ListIndex = 0    'auf ersten Eintrag setzen
    
    lstK11.Clear    'Listbox Dämpfungen leeren
    lstK12.Clear    'Listbox Dämpfungen leeren
    cboK131.Clear   'cbo Kabeltyp leeren
    'Kabeltypen eintragen
    For Y = 1 To FlexKabel1.Cols - 1 '9
        cboK131.AddItem FlexKabel1.TextMatrix(0, Y)
    Next Y
    
    cboK131.ListIndex = 0   'auf ersten Eintrag setzen

    lstK21.Clear    'Listbox Dämpfungen leeren
    lstK22.Clear    'Listbox Dämpfungen leeren
    cboK231.Clear   'cbo Kabeltyp leeren
    'Kabeltypen eintragen
    For Y = 1 To FlexKabel1.Cols - 1 '9
        cboK231.AddItem FlexKabel1.TextMatrix(0, Y)
    Next Y
    
    cboK231.ListIndex = 0   'auf ersten Eintrag setzen


End Sub



Private Sub cmdK2_click()
    
    'Eigene Kabeldaten vom Disk laden
    
'    frmKabeldaten.Show             ' Form Kabeldaten öffnen
    FlexKabel1.Cols = 29 '12           'Flexgrid kolonnen erweitern
    lblK2.Caption = "Kabelxdaten1"  'Filename für Speicherung des Files auf Harddisk
    DoKabeldaten_load              'Kabeldaten vom Disk laden
    DoKabeldatenuebertragen        'Kabeldaten von Flexgrid in Listbox übertragen
'    frmKabeldaten.refreshform      'Kabeldatenformular mit neuen Daten füllen
    frmMain.cboK31.ListIndex = 12    'auf ersten Eintrag der eigenen Kabel setzen
    frmMain.cboK131.ListIndex = 12   'auf ersten Eintrag der eigenen Kabelsetzen
    frmMain.cboK231.ListIndex = 12   'auf ersten Eintrag der eigenen Kabelsetzen

    
End Sub


'?????======= neue Kabeldaten =========


Public Sub DoKabeldaten_load()       '==== neue Kabeldaten =====

'File mit Kolonnenangabe vom Disk laden

On Error Resume Next
            
   
    FlexKabel1.Clear
  
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
    
    For YT = 0 To FlexKabel1.Rows - 1                              'ermittelt die Anzahlt der Zeilen
        
        For XT = 0 To FlexKabel1.Cols - 1                          'ermittelt die Anzahlt der Spalten
            
                    
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
    
    fGrid1.Flex1.TextMatrix(0, 0) = lblK2.Caption  'Spez für Kabeldaten-Liste erkennen
 
End Sub







'===========================Ende===================================
'neu für Zurückkopieren

Public Sub Herst_typen()

    'Hersteller auswählen beim zurückkopieren
    
    lblH1.Caption = App.Path & "\" & "Ant_Dat" & "\" & Herst & ".typ"
    
    lblH13.Caption = Int(Rnd * 100) + 2

End Sub


Public Sub SelectAntTyp()

    'Antennen Typ auswählen beim Zürückkopieren
    
    txtA2.Text = 0
    txtA6.Text = 0
    txtA7.Text = 0
    FlexGrid2.TextMatrix(12, C1) = 0
    FlexGrid2.TextMatrix(13, C1) = 0
    
    
 '==== laden der eigenen Antennen sofern eigene Antennentypen gewählt
 
    If flexT1.TextMatrix(0, 2) = "Eigene" Then
        lblT1.Caption = App.Path & "\" & "Ant_Dat" & "\" & "Eigene" & "\" & Ty & ".ant"
    Else
 
 '=====laden der Standartantennen
 
    lblT1.Caption = App.Path & "\" & "Ant_Dat" & "\" & Ty & ".ant"
    
    End If
    
    lblA7.Caption = Int(Rnd * 100) + 2

End Sub


Public Sub Winkeldämpf_anzeigen()

On Error Resume Next
    'Frequenz einlesen
    lblA2.Caption = cboA1.ItemData(cboA1.ListIndex)

    'Winkeldämpfung einlesen
    lblA16.Caption = fGrid1.MSFlex0.TextMatrix(5, 3)
 
    'Eintrag des Antennengewinns in txtA2
    
    'Sperrt die Antennenbearbeitung
    cboA1_sperren = 1
     
    'Gewinn grösser 0
    If flexA1.TextMatrix(cboA1.ListIndex + 1, 4) = Ascii Then
         txtA2.Text = 0
         
    Else
         txtA2.Text = flexA1.TextMatrix(cboA1.ListIndex + 1, 4)
         
    End If
    
 
    'Anzeigen der Winkeldämpfung
    If flexA1.TextMatrix(cboA1.ListIndex + 1, fGrid1.MSFlex0.TextMatrix(5, 3)) = Ascii Then
        txtA6.Text = 0
    Else
 
    txtA6.Text = flexA1.TextMatrix(cboA1.ListIndex + 1, fGrid1.MSFlex0.TextMatrix(5, 3))
    End If
    
    
    'Anzeigen des gewählten Winkels der Winkeldampfung
    txtA7.Text = flexA1.TextMatrix(0, fGrid1.MSFlex0.TextMatrix(5, 3))
  
  
    'Winkeldämfungen in Diagramm übertragen
    Dim wd As Integer
  
    For wd = 1 To 10
        frmS1.flexS1.Row = wd
        frmS1.flexS1.Col = 2
        frmS1.flexS1.Text = flexA1.TextMatrix(fGrid1.MSFlex0.TextMatrix(5, 3), wd + 4)
    Next

    'Hebt Sperrung der Antennenbearbeitung auf
    cboA1_sperren = 0

End Sub



'===========================================================

'============================= neu Kabel ===================



Private Sub Kabel_definieren()


    Dim xc As Integer
    Dim Xr As Integer

    FlexKabel2.Rows = 28
    FlexKabel2.Cols = 6

    For Xr = 1 To 27
        FlexKabel2.RowHeight(Xr) = 270
    Next Xr

    FlexKabel2.RowHeight(0) = 0
    FlexKabel2.ColWidth(0) = 0
    FlexKabel2.ColWidth(1) = 3500
    FlexKabel2.ColWidth(2) = 700
    FlexKabel2.ColWidth(3) = 300
    FlexKabel2.ColWidth(4) = 700
    FlexKabel2.ColWidth(5) = 300
    FlexKabel2.Height = (FlexKabel2.RowHeight(1) * FlexKabel2.Rows - 1) - 150
    FlexKabel2.Width = FlexKabel2.ColWidth(1) + FlexKabel2.ColWidth(2) + FlexKabel2.ColWidth(3) + _
    FlexKabel2.ColWidth(4) + FlexKabel2.ColWidth(5) + 50
    
    'FlexKabel2.GridLines = flexGridNone
    'FlexKabel2.GridLinesFixed = flexGridNone




    'Titelleiste fett schreiben
    FlexKabel2.Row = 1
    FlexKabel2.Col = 1
    FlexKabel2.CellFontBold = True
    
    FlexKabel2.Row = 3
    FlexKabel2.Col = 1
    FlexKabel2.CellFontBold = True

    FlexKabel2.Row = 5
    FlexKabel2.Col = 1
    FlexKabel2.CellFontBold = True

    FlexKabel2.Row = 10
    FlexKabel2.Col = 1
    FlexKabel2.CellFontBold = True

    FlexKabel2.Row = 15
    FlexKabel2.Col = 1
    FlexKabel2.CellFontBold = True

    FlexKabel2.Row = 20
    For xc = 1 To 5
    FlexKabel2.Col = xc
    FlexKabel2.CellFontBold = True
    Next xc

    FlexKabel2.Row = 25
    For xc = 1 To 5
    FlexKabel2.Col = xc
    FlexKabel2.CellFontBold = True
    Next xc



    FlexKabel2.TextMatrix(1, 1) = LoadResString(1402 + RS)
    FlexKabel2.TextMatrix(3, 1) = LoadResString(1403 + RS)
    FlexKabel2.TextMatrix(5, 1) = LoadResString(1404 + RS)
    FlexKabel2.TextMatrix(6, 1) = LoadResString(1405 + RS)
    FlexKabel2.TextMatrix(7, 1) = LoadResString(1406 + RS)
    FlexKabel2.TextMatrix(8, 1) = LoadResString(1407 + RS)
    FlexKabel2.TextMatrix(10, 1) = LoadResString(1408 + RS)
    FlexKabel2.TextMatrix(11, 1) = LoadResString(1409 + RS)
    FlexKabel2.TextMatrix(12, 1) = LoadResString(1410 + RS)
    FlexKabel2.TextMatrix(13, 1) = LoadResString(1411 + RS)
    FlexKabel2.TextMatrix(20, 1) = LoadResString(1412 + RS)
    
    FlexKabel2.TextMatrix(15, 1) = LoadResString(1431 + RS)
    FlexKabel2.TextMatrix(16, 1) = LoadResString(1432 + RS)
    FlexKabel2.TextMatrix(17, 1) = LoadResString(1433 + RS)
    FlexKabel2.TextMatrix(18, 1) = LoadResString(1434 + RS)
    FlexKabel2.TextMatrix(20, 2) = "  Total"
    FlexKabel2.TextMatrix(22, 1) = LoadResString(1413 + RS)
    FlexKabel2.TextMatrix(23, 1) = "" 'LoadResString(1420 + RS)
    FlexKabel2.TextMatrix(24, 1) = ""
    FlexKabel2.TextMatrix(25, 1) = LoadResString(1413 + RS)
    FlexKabel2.TextMatrix(25, 2) = "  Total"
    FlexKabel2.TextMatrix(27, 1) = LoadResString(1414 + RS)
    FlexKabel2.TextMatrix(27, 2) = "   Total"
    
    FlexKabel2.TextMatrix(6, 5) = "m"
    FlexKabel2.TextMatrix(6, 4) = 0
    FlexKabel2.TextMatrix(7, 5) = "dB"
    FlexKabel2.TextMatrix(7, 4) = 0
    FlexKabel2.TextMatrix(8, 3) = "m"
    FlexKabel2.TextMatrix(8, 5) = "dB"
    FlexKabel2.TextMatrix(11, 5) = "m"
    FlexKabel2.TextMatrix(11, 4) = 0
    FlexKabel2.TextMatrix(12, 5) = "dB"
    FlexKabel2.TextMatrix(12, 4) = 0
    FlexKabel2.TextMatrix(13, 3) = "m"
    FlexKabel2.TextMatrix(13, 5) = "dB"
    FlexKabel2.TextMatrix(16, 5) = "m"
    FlexKabel2.TextMatrix(16, 4) = 0
    FlexKabel2.TextMatrix(17, 5) = "dB"
    FlexKabel2.TextMatrix(17, 4) = 0
    FlexKabel2.TextMatrix(18, 3) = "m"
    FlexKabel2.TextMatrix(18, 5) = "dB"
    FlexKabel2.TextMatrix(20, 5) = "dB"
    FlexKabel2.TextMatrix(23, 5) = "dB"
    FlexKabel2.TextMatrix(23, 4) = 0
    FlexKabel2.TextMatrix(24, 5) = "dB"
    FlexKabel2.TextMatrix(24, 4) = 0
    FlexKabel2.TextMatrix(25, 5) = "dB"
    FlexKabel2.TextMatrix(27, 5) = "dB"
    

    cmdK1.Caption = LoadResString(1415 + RS)
    frm11.Caption = LoadResString(1416 + RS)
    cboK31.ToolTipText = LoadResString(1417 + RS)
    cboK51.ToolTipText = LoadResString(1419 + RS)

    FlexKabel2.Col = 4
    FlexKabel2.Row = 6
    FlexKabel2.CellBackColor = &H80FFFF
    FlexKabel2.Row = 11
    FlexKabel2.CellBackColor = &H80FFFF
    FlexKabel2.Row = 16
    FlexKabel2.CellBackColor = &H80FFFF
    FlexKabel2.Row = 23
    FlexKabel2.CellBackColor = &H80FFFF
    FlexKabel2.Row = 24
    FlexKabel2.CellBackColor = &H80FFFF
    FlexKabel2.Col = 1
    FlexKabel2.Row = 23
    FlexKabel2.CellBackColor = &H80FFFF
    FlexKabel2.Row = 24
    FlexKabel2.CellBackColor = &H80FFFF

    FlexKabel2.Col = 2
    FlexKabel2.Row = 7
    FlexKabel2.CellForeColor = vbRed
    FlexKabel2.Row = 12
    FlexKabel2.CellForeColor = vbRed
    FlexKabel2.Row = 17
    FlexKabel2.CellForeColor = vbRed


    'cboK51.Left = FlexKabel2.ColPosition(4)
    'Frequenzauswahl plazieren
    cboK51.Top = FlexKabel2.RowHeight(1) * 3 - 20
    cboK31.Top = FlexKabel2.RowHeight(1) * 5 - 20
    cboK131.Top = FlexKabel2.RowHeight(1) * 10 - 20
    cboK231.Top = FlexKabel2.RowHeight(1) * 15 - 20
    cboK51.Left = FlexKabel2.ColWidth(0) + FlexKabel2.ColWidth(1) + FlexKabel2.ColWidth(2) + _
                    FlexKabel2.ColWidth(3) + FlexKabel2.ColWidth(4) - cboK51.Width + 200
    
    cboK31.Left = FlexKabel2.ColWidth(0) + FlexKabel2.ColWidth(1) + FlexKabel2.ColWidth(2) + _
                    FlexKabel2.ColWidth(3) + FlexKabel2.ColWidth(4) - cboK131.Width + 200
    cboK131.Left = cboK31.Left
    cboK231.Left = cboK31.Left


End Sub


Private Sub FlexKabel2_Click()


    'Cursor positinieren
    If FlexKabel2.Row >= 1 Then
        If FlexKabel2.Row < 22 Then
            FlexKabel2.Col = 4
        End If
    End If
    
    If FlexKabel2.Row >= 1 Then
        If FlexKabel2.Row < 6 Then
            FlexKabel2.Row = 6
        End If
    End If

    If FlexKabel2.Row > 6 Then
        If FlexKabel2.Row < 11 Then
            FlexKabel2.Row = 11
        End If
    End If

    If FlexKabel2.Row > 11 Then
        If FlexKabel2.Row < 16 Then
            FlexKabel2.Row = 16
        End If
    End If
    
    If FlexKabel2.Row > 16 Then
        If FlexKabel2.Row < 23 Then
            FlexKabel2.Row = 23
        End If
    End If

    If FlexKabel2.Row > 24 Then
        If FlexKabel2.Row <= 28 Then
           ' FlexKabel2.Col = 1
            FlexKabel2.Row = 24
        End If
    End If
     
    If FlexKabel2.Row = 23 Then
        If FlexKabel2.Col >= 2 Then
            FlexKabel2.Col = 4
            FlexKabel2.Row = 23
        End If
    End If
    
    If FlexKabel2.Row = 23 Then
        If FlexKabel2.Col = 1 Then
            FlexKabel2.Col = 1
            FlexKabel2.Row = 23
        End If
    End If
    
   
    If FlexKabel2.Row = 24 Then
        If FlexKabel2.Col >= 2 Then
            FlexKabel2.Col = 4
            FlexKabel2.Row = 24
        End If
    End If
  
     If FlexKabel2.Row = 24 Then
        If FlexKabel2.Col = 1 Then
            FlexKabel2.Col = 1
            FlexKabel2.Row = 24
        End If
    End If
 
    

    'Felder von FlexKabel2 editieren
    '===============================
    'Spez. Eintrag im Klassenmodul für FlexKabel2.Col = 1 "mit Text füllbar"
    'umgehung der nummerischen Eingabe
    
    'Aufruf des Klassen moduls
    Set clsFGEdit2 = New clsFlexGridEdit2
    Set clsFGEdit2.FlexGridControl = FlexKabel2


    
End Sub



Public Sub Kabel_berechnen()

'Kabeldaten berechnen nd übrige Dämpfung

    Dim k1x As Double 'Integer
    Dim k2x As Double 'Integer
    Dim k3x As Double 'Integer
    Dim k4x As Double 'Integer
    Dim k5x As Double 'Integer

    'Kabellisten löschen
    lstK1.Clear
    lstK2.Clear
    lstK11.Clear
    lstK12.Clear
    lstK21.Clear
    lstK22.Clear


On Error Resume Next

    'Kabellängen übernehmen (initialisierung)
    FlexKabel2.TextMatrix(6, 4) = Format(FlexKabel2.TextMatrix(6, 4), "0.00")
    FlexKabel2.TextMatrix(11, 4) = Format(FlexKabel2.TextMatrix(11, 4), "0.00")
    FlexKabel2.TextMatrix(16, 4) = Format(FlexKabel2.TextMatrix(16, 4), "0.00")
    FlexKabel2.TextMatrix(8, 2) = FlexKabel2.TextMatrix(6, 4)
    FlexKabel2.TextMatrix(13, 2) = FlexKabel2.TextMatrix(11, 4)
    FlexKabel2.TextMatrix(18, 2) = FlexKabel2.TextMatrix(16, 4)
   
'Kabeltyp 1
    'Listen füllen lstK1=Frequenz, lstK2=Dämpfung

    Dim fa As Integer           'Tabellen-Index der Frequenz
        fa = cboK51.ListIndex

    Dim Ds As Integer           'Tabellen-Index der Kabeltypen 1
        Ds = cboK31.ListIndex

    Dim t As Integer
    For t = 1 To FlexKabel1.Rows - 1 '14
       lstK1.AddItem (cboK51.List(t - 1)), (t - 1)
       lstK2.AddItem (Kabel(t - 1, Ds)), (t - 1)
    Next t

    'Kabelliste 1  auslesen
    FlexKabel2.TextMatrix(7, 4) = Kabel(fa, Ds)
        
    'Wenn Keine Kabeldaten vorhanden, dann überspringen
    If FlexKabel2.TextMatrix(7, 4) = "" Then
        FlexKabel2.TextMatrix(7, 2) = "No Data"
    Else
        FlexKabel2.TextMatrix(7, 2) = ""
    End If
    

    'Berechnen
  '  FlexKabel2.TextMatrix(6, 4) = Format(FlexKabel2.TextMatrix(6, 4), "0.00")
    FlexKabel2.TextMatrix(8, 4) = Format(FlexKabel2.TextMatrix(7, 4) * FlexKabel2.TextMatrix(6, 4) / 100, "0.00")


'Kabeltyp 2
    ' lstK11 und lstK12 füllen

    Dim Da As Integer           'Tabellen-Index der Kabeltypen 2
        Da = cboK131.ListIndex
    Dim s As Integer
    For s = 1 To FlexKabel1.Rows - 1 '14
        lstK11.AddItem (cboK51.List(s - 1)), (s - 1)
        lstK12.AddItem (Kabel(s - 1, Da)), (s - 1)

    Next s

    'Kabeltyp 2 auslesen
    FlexKabel2.TextMatrix(12, 4) = Kabel(fa, Da)
     
    'Wenn Keine Kabeldaten vorhanden, dann überspringen
    If FlexKabel2.TextMatrix(12, 4) = "" Then
        FlexKabel2.TextMatrix(12, 2) = "No Data"
    Else
        FlexKabel2.TextMatrix(12, 2) = ""
    End If
    
   
    'Berechnen
'    FlexKabel2.TextMatrix(11, 4) = Format(FlexKabel2.TextMatrix(11, 4), "0.00")
    FlexKabel2.TextMatrix(13, 4) = Format(FlexKabel2.TextMatrix(12, 4) * FlexKabel2.TextMatrix(11, 4) / 100, "0.00")


'Kabeltyp 3
    ' lstK21 und lstK22 füllen

    Dim Ca As Integer           'Tabellen-Index der Kabeltypen 3
        Ca = cboK231.ListIndex
    Dim u As Integer
    For u = 1 To FlexKabel1.Rows - 1 '14
        lstK21.AddItem (cboK51.List(u - 1)), (u - 1)
        lstK22.AddItem (Kabel(u - 1, Ca)), (u - 1)

    Next u

    'Kabeltyp 3 auslesen
    
    FlexKabel2.TextMatrix(17, 4) = Kabel(fa, Ca)
    
    'Wenn Keine Kabeldaten vorhanden, dann überspringen
    If FlexKabel2.TextMatrix(17, 4) = "" Then
        FlexKabel2.TextMatrix(17, 2) = "No Data"
    Else
        FlexKabel2.TextMatrix(17, 2) = ""
    End If
    
    'Berechnen
'    FlexKabel2.TextMatrix(16, 4) = Format(FlexKabel2.TextMatrix(16, 4), "0.00")
    FlexKabel2.TextMatrix(18, 4) = Format(FlexKabel2.TextMatrix(17, 4) * FlexKabel2.TextMatrix(16, 4) / 100, "0.00")

    'Wenn kein Text eingefügt in übrige Dämpfung
    If FlexKabel2.TextMatrix(23, 1) = "" Then
        FlexKabel2.TextMatrix(23, 4) = 0
    Else
    End If

    If FlexKabel2.TextMatrix(24, 1) = "" Then
        FlexKabel2.TextMatrix(24, 4) = 0
    Else
    End If


    'Total Kabeldämpfung
    
    k1x = FlexKabel2.TextMatrix(8, 4)
    k2x = FlexKabel2.TextMatrix(13, 4)
    k3x = FlexKabel2.TextMatrix(18, 4)
    
    FlexKabel2.TextMatrix(23, 4) = Format(FlexKabel2.TextMatrix(23, 4), "0.00")
    k4x = FlexKabel2.TextMatrix(23, 4)
    
    FlexKabel2.TextMatrix(24, 4) = Format(FlexKabel2.TextMatrix(24, 4), "0.00")
    k5x = FlexKabel2.TextMatrix(24, 4)
    
    
    FlexKabel2.TextMatrix(20, 4) = ""
    FlexKabel2.TextMatrix(20, 4) = Format(k1x + k2x + k3x, "0.00")

    'Total übrige Dämpfung
    FlexKabel2.TextMatrix(25, 4) = Format(k4x + k5x, "0.00")


    'Total Dämpfung
    FlexKabel2.TextMatrix(27, 4) = k1x + k2x + k3x + k4x + k5x


    'Höhe Listbox festlegen
    lstK1.Height = 190 * FlexKabel1.Rows - 1
    lstK2.Height = 190 * FlexKabel1.Rows - 1


    'Kabellänge: Eintrag 0 wenn keine Zahl in  eingetragen ist
    If FlexKabel2.TextMatrix(7, 4) = Ascii Then
        FlexKabel2.TextMatrix(7, 4) = 0
    End If
    
    If FlexKabel2.TextMatrix(12, 4) = Ascii Then
        FlexKabel2.TextMatrix(12, 4) = 0
    End If
    
    If FlexKabel2.TextMatrix(17, 4) = Ascii Then
        FlexKabel2.TextMatrix(17, 4) = 0
    End If

    'übertragen des Wertes Kabel- und übrige-Dämpfung auf Hauptformular
    FlexGrid2.TextMatrix(8, C1) = Format(k1x + k2x + k3x, "0.00")
    FlexGrid2.TextMatrix(9, C1) = Format(k4x + k5x, "0.00")

'    'Aendern der Farbe von cmd03
    lbl76.Caption = Int(Rnd * 100) + 2


End Sub

'==============================Ende===================================




