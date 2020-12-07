Imports System.IO
Imports System.Text
Imports System.Math
Imports System.Xml.XPath

Imports VGM2MID.Util
Imports System.IO.Compression

Public Class Form1

    ' VGM Info Struct
    Private Structure VGMFile_Struct
        Dim lenVGM As Integer
        Dim verVGM As Integer
        Dim iDataOffset As Integer

        ' Sound Chip Count (Depends on VGM File Version)
        Dim iCntChip As Integer
        ' Sound Chip Info
        Dim szSndChip As String
        ' Sound Chip Frequency
        Dim iFreqChip() As Integer
        ' Sound Chip Active
        Dim fActiveChip() As Boolean
        ' Dual Sound Chip Support
        Dim fDualChip() As Boolean

    End Structure

    Private Structure GD3Tag_Struct

        Dim verGD3 As String
        Dim lenGD3 As Integer

        Dim enTrkName, jpTrkName As String
        Dim enGameName, jpGameName As String
        Dim enSysName, jpSysName As String
        Dim enAuthor, jpAuthor As String
        Dim rlsDate As String
        Dim vgmMaker As String
        Dim strNotes As String

    End Structure

    Private Structure Operator_Struct

        ' Attack Rate
        Dim AR As Integer
        ' Decay Rate 1
        Dim D1R As Integer
        ' Decay Rate 2
        Dim D2R As Integer
        ' Release Rate
        Dim RR As Integer
        ' Sustain Rate
        Dim SR As Integer

        ' Decay Level 1
        Dim D1L As Integer
        ' Total Level
        Dim TL As Integer
        ' Key Scaling
        Dim KS As Integer
        ' Multiplier
        Dim MUL As Integer

        ' Fine Detuning
        Dim DT1 As Integer
        ' Coarse Detuning
        Dim DT2 As Integer
        ' SSG Envelope
        Dim SSGEG As Integer

        ' Amplitude Modulation Enable
        Dim AME As Integer

    End Structure

    Private Structure Voice_Struct
        ' Voice Name
        Dim Name As String

        ' LFO (Low Frequency Oscillator) Frequency
        Dim LFRQ As Integer
        ' Amplitude Modulation
        Dim AMD As Integer
        ' Phase Modulation
        Dim PMD As Integer
        ' WaveForm
        Dim WF As Integer
        ' Enable Low Frequency Oscillator
        Dim NFRQ As Integer

        ' *** Unknown and Unused ***
        Dim PAN As Integer
        ' FeedBack Loop
        Dim FL As Integer
        ' Connection, aka Algorithm
        Dim CON As Integer
        ' Amplitude Modulation Sensitivity
        Dim AMS As Integer
        ' Phase Modulation Sensitivity
        Dim PMS As Integer
        ' Slot Mask
        Dim SLOT As Integer
        ' Noise Enable
        Dim NE As Integer

        ' Operator (M1, C1, M2, C2)
        Dim Op() As Operator_Struct

    End Structure

    Private Structure SSGVoice_Struct
        ' SSG Voice Name
        Dim Name As String

        ' Envelope/Noise/Tone Flag
        Dim ENTFlag() As Boolean

    End Structure

    ' -1 = Not Exist
    Private Structure FMVoiceMapper_Struct
        ' Add Channel Count and Operator Count
        Dim iChannelCount As Integer
        Dim iOPCount As Integer

        Dim iRegLFRQ As Integer
        Dim iRegAMD As Integer
        Dim iRegPMD As Integer
        Dim iRegWF As Integer
        Dim iRegNFRQ As Integer

        Dim iRegPAN As Integer
        Dim iRegFL As Integer
        Dim iRegCON As Integer
        Dim iRegAMS As Integer
        Dim iRegPMS As Integer
        Dim iRegSLOT As Integer
        Dim iRegNE As Integer

        ' Start Address
        ' [Operator Mask]
        Dim iOPRegAR As Integer
        Dim iOPRegD1R As Integer
        Dim iOPRegD2R As Integer
        Dim iOPRegSR As Integer
        Dim iOPRegRR As Integer
        Dim iOPRegD1L As Integer
        Dim iOPRegTL As Integer
        Dim iOPRegKS As Integer
        Dim iOPRegMUL As Integer
        Dim iOPRegDT1 As Integer
        Dim iOPRegDT2 As Integer
        Dim iOPRegAME As Integer

        ' Constructer
        Public Sub New(
            iCC As Integer, iOC As Integer,
            LFRQ As Integer, AMD As Integer, PMD As Integer, WF As Integer, NFRQ As Integer,
            PAN As Integer, FL As Integer, CON As Integer, AMS As Integer, PMS As Integer, SLOT As Integer, NE As Integer,
            AR As Integer, D1R As Integer, D2R As Integer, SR As Integer, RR As Integer, D1L As Integer, TL As Integer, KS As Integer, MUL As Integer, DT1 As Integer, DT2 As Integer, AME As Integer
        )
            iChannelCount = iCC : iOPCount = iOC

            iRegLFRQ = LFRQ : iRegAMD = AMD : iRegPMD = PMD : iRegWF = WF : iRegNFRQ = NFRQ

            iRegPAN = PAN : iRegFL = FL : iRegCON = CON : iRegAMS = AMS
            iRegPMS = PMS : iRegSLOT = SLOT : iRegNE = NE

            iOPRegAR = AR : iOPRegD1R = D1R : iOPRegD2R = D2R : iOPRegSR = SR : iOPRegRR = RR
            iOPRegD1L = D1L : iOPRegTL = TL : iOPRegKS = KS : iOPRegMUL = MUL
            iOPRegDT1 = DT1 : iOPRegDT2 = DT2 : iOPRegAME = AME
        End Sub

    End Structure

    Private Structure CurrVoice_Struct
        Dim VolumeChangeAmount As Integer
        Dim Voice As Voice_Struct
    End Structure

    Private Enum chipTag As Integer
        SND_SN76489 = 3
        SND_YM2143
        SND_YM2612 = 11
        SND_YM2151
        SND_SEGAPCM = 14
        SND_RF5C68 = 16
        SND_YM2203
        SND_YM2608
        SND_YM2610
        SND_YM3812
        SND_YM3526
        SND_Y8950
        SND_YMF262
        SND_F278B
        SND_F271
        SND_YMZ280B
        SND_RF5C164
        SND_PWM
        SND_AY8910
        SND_GBDMG = 32
        SND_NESAPU
        SND_MULTIPCM
        SND_UPD7759
        SND_OKIM6258
        SND_OKIM6295 = 38
        SND_K051649
        SND_K054539
        SND_HUC6280
        SND_C140
        SND_K053260
        SND_POKEY
        SND_QSOUND
        SND_SCSP
        SND_WONDERSWAN = 48
        SND_VSU
        SND_SAA1099
        SND_ES5503
        SND_ES5506
        SND_X1010 = 54
        SND_C352
        SND_GA20
    End Enum

    Private Enum idFMVoiceMapper As Integer
        ID_YM2151
        ID_YM2203
    End Enum

    ' Predefined Address In VGM Files
    Const ADDR_FILESIZE As Integer = &H4
    Const ADDR_VGMVER As Integer = &H8
    Const ADDR_GD3OFFSET As Integer = &H14
    Const ADDR_YM2151 As Integer = &H30
    Const ADDR_DATAOFFSET As Integer = &H34

    Dim chipMappingCheck As Boolean() = New Boolean(63) {
        False, False, False, True, True, False, False, False,
        False, False, False, True, True, False, True, False,
        True, True, True, True, True, True, True, True,
        True, True, True, True, True, True, False, False,
        True, True, True, True, True, False, True, True,
        True, True, True, True, True, True, True, False,
        True, True, True, True, True, False, True, True,
        True, False, False, False, False, False, False, False
    }
    Dim chipMapping As String() = New String(63) {
        "", "", "", "SN76489",
        "YM2143", "", "", "",
        "", "", "", "YM2612",
        "YM2151", "", "Sega PCM", "",
        "RF5C68", "YM2203", "YM2608", "YM2610/B",
        "YM3812", "YM3526", "Y8950", "YMF262",
        "YMF278B", "YMF271", "YMZ280B", "RF5C164",
        "PWM", "AY8910", "", "",
        "GB DMG", "NES APU", "MultiPCM", "uPD7759",
        "OKIM6258", "", "OKIM6295", "K051649",
        "K054539", "HuC6280", "C140", "K053260",
        "Pokey", "QSound", "SCSP", "",
        "WonderSwan", "VSU", "SAA1099", "ES5503",
        "ES5506", "", "X1-010", "C352",
        "GA20", "", "", "",
        "", "", "", ""
    }

    ' Register Address Mapper
    ' iChannelCount, iOPCount
    ' iRegLFRQ, iRegAMD, iRegPMD, iRegWF, iRegNFRQ
    ' iRegPan, iRegFL, iRegCON, iRegAMS, iRegPMS, iRegSlot, iRegNE
    ' iRegAR, iRegD1R, iRegD2R, iRegSR, iRegRR, iRegD1L, iRegTL, iRegKS, iRegMUL, iRegDT1, iRegDT2, iRegAME
    Dim FMVoiceMapper As FMVoiceMapper_Struct() = New FMVoiceMapper_Struct(1) {
        New FMVoiceMapper_Struct(
            &H8, &H4,
            &H18, &H19, &H19, &H1B, &HF,
            &H20, &H20, &H20, &H38, &H38, &H8, &HF,
            &H80, &HA0, &HC0, -1, &HE0, &HE0, &H60, &H80, &H40, &H40, &HC0, &HA0
        ), ' OP-M
        New FMVoiceMapper_Struct(
            &H3, &H4,
            -1, -1, -1, -1, &H22,
            -1, &HB0, &HB0, &HB4, &HB4, &H28, -1,
            &H50, &H60, -1, &H70, &H80, -1, &H40, &H50, &H30, &H30, -1, -1
        )  ' OP-N
    }

    ' "A-0", "A#0", "B-0",
    ' "C-1", "C#1", "D-1", "D#1", "E-1", "F-1", "F#1", "G-1", "G#1", "A-1", "A#1", "B-1",
    ' "C-2", "C#2", "D-2", "D#2", "E-2", "F-2", "F#2", "G-2", "G#2", "A-2", "A#2", "B-2",
    ' "C-3", "C#3", "D-3", "D#3", "E-3", "F-3", "F#3", "G-3", "G#3", "A-3", "A#3", "B-3",
    ' "C-4", "C#4", "D-4", "D#4", "E-4", "F-4", "F#4", "G-4", "G#4", "A-4", "A#4", "B-4",
    ' "C-5", "C#5", "D-5", "D#5", "E-5", "F-5", "F#5", "G-5", "G#5", "A-5", "A#5", "B-5",
    ' "C-6", "C#6", "D-6", "D#6", "E-6", "F-6", "F#6", "G-6", "G#6", "A-6", "A#6", "B-6",
    ' "C-7", "C#7", "D-7", "D#7", "E-7", "F-7", "F#7", "G-7", "G#7", "A-7", "A#7", "B-7",
    ' "C-8",
    Dim MIDINoteFreqTable As Double() = {
        27.5, 29.135, 30.868,
        32.703, 34.648, 36.708, 38.891, 41.203, 43.654, 46.249, 48.999, 51.913, 55.0, 58.27, 61.735,
        65.406, 69.296, 73.416, 77.782, 82.407, 87.307, 92.499, 97.999, 103.83, 110.0, 116.54, 123.47,
        130.81, 138.59, 146.83, 155.56, 164.81, 174.61, 185.0, 196.0, 207.85, 220.0, 233.08, 246.94,
        261.63, 277.18, 293.67, 311.13, 329.63, 349.23, 369.99, 392.0, 415.3, 440.0, 466.16, 493.88,
        523.25, 554.37, 587.33, 622.25, 659.26, 698.46, 739.99, 783.99, 830.61, 880.0, 932.33, 987.77,
        1046.5, 1108.7, 1174.7, 1244.5, 1318.5, 1396.9, 1480.0, 1568.0, 1661.2, 1760.0, 1864.7, 1975.5,
        2093.0, 2217.5, 2349.3, 2489.0, 2637.0, 2793.0, 2960.0, 3136.0, 3322.4, 3520.0, 3729.3, 3951.1,
        4186.0
    }

    Dim VolumeTable_YM2149 As Integer() = {
        0, 0, 1, 1, 2, 2, 3, 3,
        5, 5, 7, 7, 11, 11, 15, 15,
        22, 22, 31, 31, 45, 45, 63, 63,
        90, 90, 127, 127, 180, 180, 255, 255
    }

    ' *** Global Variables ***

    Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
    Dim in_file As MemoryStream
    Dim szOrigFileName As String
    Dim filepos As Integer
    Dim filelength As Integer
    Dim d(3) As Byte
    Dim temp As Integer
    Dim delay As Integer = 0
    Dim ym_reg As Integer
    Dim ym_val As Integer

    Dim Note(1, 7) As Integer
    Dim Note_Old(1, 7) As Integer
    Dim NoteOn(1, 7) As Boolean
    Dim NoteOn_Old(1, 7) As Boolean
    Dim Slot(1, 7) As Integer

    ' Used By OPM
    Dim KF(7) As Integer
    Dim KF_old(7) As Integer

    ' Used By OPN
    Dim FNumLow(1, 2) As Integer
    Dim FNumHigh(1, 2) As Integer

    ' Used By SSG
    Dim SSGNote(1, 2) As Integer
    Dim SSGNote_Old(1, 2) As Integer
    Dim SSGNoteOn(1, 2) As Boolean
    Dim SSGNoteOn_Old(1, 2) As Boolean
    Dim SSGMix(1, 2) As Boolean
    Dim SSGVolume(1, 2) As Integer
    Dim SSGVolume_Old(1, 2) As Integer
    Dim SSGTuneFine(1, 2) As Integer
    Dim SSGTuneCoarse(1, 2) As Integer

    ' Pitch Wheel
    Dim PitchWheel As Integer(,) = {{&H2000, &H2000, &H2000}, {&H2000, &H2000, &H2000}}
    Dim LastPitchWheel As Integer(,) = {{&H2000, &H2000, &H2000}, {&H2000, &H2000, &H2000}}

    Dim DataBlockID As Integer = 0

    Dim MIDIByteCount As Integer
    Dim out_file_midi As FileStream
    Dim out_file_syx As FileStream
    Dim out_file_opm As StreamWriter

    ' 0x00 - 0xFF Registers
    Dim Registers(1023) As Byte
    Dim AMD As Byte
    Dim PMD As Byte
    Dim RegisterChanged As Boolean

    Dim CurrentVoice(1, 7) As CurrVoice_Struct
    Dim Voices() As Voice_Struct
    Dim VoicesCount As Integer
    Dim VoiceID(1, 7) As Integer
    Dim VoiceID_old(1, 7) As Integer
    Dim VolumeChangeAmount_old(1, 7) As Integer
    Dim TL_Tol As Integer
    Dim MaxVol As Double
    Dim Gain As Double
    Dim BPM As Double
    Dim TQN As Integer

    Dim OnceFlag As Boolean(,) = {{False, False, False}, {False, False, False}}

    Dim VGMFile As VGMFile_Struct
    Dim GD3Tag As GD3Tag_Struct

    Private Sub WriteDataBlock(iDataType As Integer, szFileName As String, byteData As Byte(), iDataLength As Integer)
        Dim pOutFileStream As FileStream
        Dim szOutFileName As String = szFileName
        Dim szOutFileExt As String = ""

        Select Case (iDataType)
            Case &H82
                szOutFileExt = ".pcma"
            Case &H83
                szOutFileExt = ".pcmb"
            Case Else
                Return
        End Select

        pOutFileStream = New FileStream(szOutFileName & "_" & DataBlockID & szOutFileExt, FileMode.Create)
        pOutFileStream.Write(byteData, 0, iDataLength)
        pOutFileStream.Close()

        DataBlockID = DataBlockID + 1

    End Sub

    Private Sub Send_Midi(Command As Integer, Param1 As Integer, Param2 As Integer)

        Dim t(3) As Byte
        Dim delay2 As Integer
        Dim delay3 As Double

        delay3 = 44100 / TQN
        delay3 = delay3 * 60 / BPM
        delay3 = delay / delay3

        delay2 = delay3

        t(0) = (delay2 >> 21) And 127
        t(1) = (delay2 >> 14) And 127
        t(2) = (delay2 >> 7) And 127
        t(3) = delay2 And 127

        If t(0) <> 0 Then out_file_midi.WriteByte(t(0) Or 128) : MIDIByteCount = MIDIByteCount + 1
        If t(1) <> 0 Then out_file_midi.WriteByte(t(1) Or 128) : MIDIByteCount = MIDIByteCount + 1
        If t(2) <> 0 Then out_file_midi.WriteByte(t(2) Or 128) : MIDIByteCount = MIDIByteCount + 1
        out_file_midi.WriteByte(t(3)) : MIDIByteCount = MIDIByteCount + 1

        delay = 0

        If (Command <> -1) Then out_file_midi.WriteByte(Command) : MIDIByteCount = MIDIByteCount + 1
        If (Param1 <> -1) Then out_file_midi.WriteByte(Param1) : MIDIByteCount = MIDIByteCount + 1
        If (Param2 <> -1) Then out_file_midi.WriteByte(Param2) : MIDIByteCount = MIDIByteCount + 1

    End Sub

    Private Function CompareVoice(v1 As Voice_Struct, v2 As Voice_Struct) As Boolean

        Dim op As Integer

        If (v1.LFRQ <> v2.LFRQ) Then Return (False)
        If (v1.AMD <> v2.AMD) Then Return (False)
        If (v1.PMD <> v2.PMD) Then Return (False)
        If (v1.WF <> v2.WF) Then Return (False)
        If (v1.NFRQ <> v2.NFRQ) Then Return (False)

        If (v1.PAN <> v2.PAN) Then Return (False)
        If (v1.FL <> v2.FL) Then Return (False)
        If (v1.CON <> v2.CON) Then Return (False)
        If (v1.AMS <> v2.AMS) Then Return (False)
        If (v1.PMS <> v2.PMS) Then Return (False)
        If (v1.SLOT <> v2.SLOT) Then Return (False)
        If (v1.NE <> v2.NE) Then Return (False)

        For op = 0 To 3

            If (v1.Op(op).AR <> v2.Op(op).AR) Then Return (False)
            If (v1.Op(op).D1R <> v2.Op(op).D1R) Then Return (False)
            If (v1.Op(op).D2R <> v2.Op(op).D2R) Then Return (False)
            If (v1.Op(op).SR <> v2.Op(op).SR) Then Return (False)
            If (v1.Op(op).RR <> v2.Op(op).RR) Then Return (False)
            If (v1.Op(op).D1L <> v2.Op(op).D1L) Then Return (False)
            ' If (Math.Abs(v1.Op(op).TL - v2.Op(op).TL) > TL_Tol) Then Return (False)
            ' If (v1.Op(op).KS <> v2.Op(op).KS) Then Return (False)
            ' If (v1.Op(op).MUL <> v2.Op(op).MUL) Then Return (False)
            ' If (v1.Op(op).DT1 <> v2.Op(op).DT1) Then Return (False)
            If (v1.Op(op).DT2 <> v2.Op(op).DT2) Then Return (False)
            ' If (v1.Op(op).SSGEG <> v2.Op(op).SSGEG) Then Return (False)
            If (v1.Op(op).AME <> v2.Op(op).AME) Then Return (False)

        Next

        Return (True)

    End Function

    Private Function FindVoice(v As Voice_Struct) As Integer

        Dim i As Integer
        Dim found As Integer

        found = -1
        If (VoicesCount > 0) Then
            i = 0
            Do
                If (CompareVoice(v, Voices(i))) Then found = i
                i = i + 1
            Loop Until ((found <> -1) Or (i >= VoicesCount))
        End If

        If (found = -1) Then
            AddVoice(v)
            found = VoicesCount - 1
        End If

        Return (found)

    End Function

    Private Sub AddVoice(v As Voice_Struct)

        VoicesCount = VoicesCount + 1

        ReDim Preserve Voices(VoicesCount - 1)

        Voices(VoicesCount - 1) = v

    End Sub

    ' YM2151 Version 

    Private Function GetCurrentVoice(chan As Integer) As CurrVoice_Struct

        Dim curr_voice As CurrVoice_Struct
        Dim op As Integer
        Dim TL_Min As Integer

        'Dim TL_Change_Count As Integer

        ReDim curr_voice.Voice.Op(3)

        curr_voice.Voice.Name = ""

        curr_voice.Voice.AMS = Registers(&H38 + chan) And 3
        curr_voice.Voice.PMS = (Registers(&H38 + chan) >> 4) And 7

        If ((curr_voice.Voice.AMS <> 0) Or (curr_voice.Voice.PMS <> 0)) Then
            curr_voice.Voice.LFRQ = Registers(&H18)
            curr_voice.Voice.AMD = AMD
            curr_voice.Voice.PMD = PMD
        Else
            curr_voice.Voice.LFRQ = 0
            curr_voice.Voice.AMD = 0
            curr_voice.Voice.PMD = 0
        End If

        curr_voice.Voice.WF = Registers(&H1B) And 3
        curr_voice.Voice.NFRQ = Registers(&HF) And 127
        curr_voice.Voice.PAN = Registers(&H20 + chan) And 192
        curr_voice.Voice.FL = (Registers(&H20 + chan) >> 3) And 7
        curr_voice.Voice.CON = Registers(&H20 + chan) And 7
        curr_voice.Voice.SLOT = Registers(&H8) And 120
        curr_voice.Voice.NE = Registers(&HF) And 128

        For op = 0 To 3

            curr_voice.Voice.Op(op).AR = Registers(&H80 + chan + (op * 8)) And 31
            curr_voice.Voice.Op(op).D1R = Registers(&HA0 + chan + (op * 8)) And 31
            curr_voice.Voice.Op(op).D2R = Registers(&HC0 + chan + (op * 8)) And 31
            curr_voice.Voice.Op(op).RR = Registers(&HE0 + chan + (op * 8)) And 15
            curr_voice.Voice.Op(op).D1L = (Registers(&HE0 + chan + (op * 8)) >> 4) And 15
            curr_voice.Voice.Op(op).TL = Registers(&H60 + chan + (op * 8)) And 127
            curr_voice.Voice.Op(op).KS = (Registers(&H80 + chan + (op * 8)) >> 6) And 3
            curr_voice.Voice.Op(op).MUL = Registers(&H40 + chan + (op * 8)) And 15
            curr_voice.Voice.Op(op).DT1 = (Registers(&H40 + chan + (op * 8)) >> 4) And 7
            curr_voice.Voice.Op(op).DT2 = (Registers(&HC0 + chan + (op * 8)) >> 6) And 3
            curr_voice.Voice.Op(op).AME = Registers(&HA0 + chan + (op * 8)) And 128

        Next

        'maximise volume
        TL_Min = 255
        'TL_Change_Count = 0
        For op = 0 To 3
            If (Carrier(curr_voice.Voice.CON, op)) Then
                If (TL_Min > curr_voice.Voice.Op(op).TL) Then TL_Min = curr_voice.Voice.Op(op).TL
            End If
        Next

        If (TL_Min <= 127) Then

            For op = 0 To 3
                If (Carrier(curr_voice.Voice.CON, op)) Then
                    curr_voice.Voice.Op(op).TL = curr_voice.Voice.Op(op).TL - TL_Min
                    'TL_Change_Count = TL_Change_Count + 1
                End If
            Next

            curr_voice.VolumeChangeAmount = TL_Min
            'ListBox2.Items.Add("VolumeChangeAmount= " + TL_Min.ToString)

        Else
            curr_voice.VolumeChangeAmount = 0
            ListBox2.Items.Add("Did not find minimum TL")
        End If

        Return (curr_voice)

    End Function

    Private Function GetCurrentVoiceYM2203(chip As Integer, chan As Integer) As CurrVoice_Struct

        Dim curr_voice As CurrVoice_Struct
        Dim op As Integer
        Dim TL_Min As Integer

        'Dim TL_Change_Count As Integer

        ReDim curr_voice.Voice.Op(3)

        curr_voice.Voice.Name = ""

        curr_voice.Voice.LFRQ = 0
        curr_voice.Voice.AMD = 0
        curr_voice.Voice.PMD = 0
        curr_voice.Voice.WF = 0
        curr_voice.Voice.NFRQ = 0
        curr_voice.Voice.PAN = 0
        ' FeedBack
        curr_voice.Voice.FL = (Registers(&HB0 + chan + 256 * chip) >> 3) And 7
        ' Connect
        curr_voice.Voice.CON = Registers(&HB0 + chan + 256 * chip) And 7
        ' Slot
        curr_voice.Voice.SLOT = (Registers(&H28 + 256 * chip) >> 4) And &HF
        curr_voice.Voice.NE = 0

        For op = 0 To 3

            curr_voice.Voice.Op(op).AR = Registers(&H50 + chan + (op * 8) + 256 * chip) And 31
            curr_voice.Voice.Op(op).D1R = Registers(&H60 + chan + (op * 8) + 256 * chip) And 31
            curr_voice.Voice.Op(op).D2R = 0
            curr_voice.Voice.Op(op).RR = Registers(&H80 + chan + (op * 8) + 256 * chip) And 15
            curr_voice.Voice.Op(op).D1L = 0
            curr_voice.Voice.Op(op).TL = Registers(&H40 + chan + (op * 8) + 256 * chip) And 127
            curr_voice.Voice.Op(op).KS = (Registers(&H50 + chan + (op * 8) + 256 * chip) >> 6) And 3
            curr_voice.Voice.Op(op).MUL = Registers(&H30 + chan + (op * 8) + 256 * chip) And 15
            curr_voice.Voice.Op(op).DT1 = (Registers(&H30 + chan + (op * 8) + 256 * chip) >> 4) And 7
            curr_voice.Voice.Op(op).DT2 = 0
            curr_voice.Voice.Op(op).SSGEG = Registers(&H90 + chan + (op * 8) + 256 * chip) And 15
            curr_voice.Voice.Op(op).AME = 0
        Next

        'maximise volume
        TL_Min = 255
        'TL_Change_Count = 0
        For op = 0 To 3
            If (Carrier(curr_voice.Voice.CON, op)) Then
                If (TL_Min > curr_voice.Voice.Op(op).TL) Then TL_Min = curr_voice.Voice.Op(op).TL
            End If
        Next

        If (TL_Min <= 127) Then

            For op = 0 To 3
                If (Carrier(curr_voice.Voice.CON, op)) Then
                    curr_voice.Voice.Op(op).TL = curr_voice.Voice.Op(op).TL - TL_Min
                    'TL_Change_Count = TL_Change_Count + 1
                End If
            Next

            curr_voice.VolumeChangeAmount = TL_Min
            'ListBox2.Items.Add("VolumeChangeAmount= " + TL_Min.ToString)

        Else
            curr_voice.VolumeChangeAmount = 0
            ListBox2.Items.Add("Did not find minimum TL")
        End If

        Return (curr_voice)

    End Function

    Private Function GetCurrentVoiceYM2608(chan As Integer, port As Integer) As CurrVoice_Struct

        Dim curr_voice As CurrVoice_Struct
        Dim i, op As Integer
        Dim TL_Min As Integer

        'Dim TL_Change_Count As Integer

        ReDim curr_voice.Voice.Op(3)

        curr_voice.Voice.Name = ""

        curr_voice.Voice.AMS = (Registers(&HB4 + chan + 256 * port) >> 4) And 3
        curr_voice.Voice.PMS = Registers(&HB4 + chan + 256 * port) And 7

        curr_voice.Voice.LFRQ = 0
        curr_voice.Voice.AMD = 0
        curr_voice.Voice.PMD = 0
        curr_voice.Voice.WF = 0
        curr_voice.Voice.NFRQ = 0
        curr_voice.Voice.PAN = 0
        ' FeedBack
        curr_voice.Voice.FL = ((Registers(&HB0 + chan) >> 3) + 256 * port) And 7
        ' Connect
        curr_voice.Voice.CON = (Registers(&HB0 + chan) + 256 * port) And 7
        ' Slot
        curr_voice.Voice.SLOT = ((Registers(&H28) >> 4)) And &HF
        curr_voice.Voice.NE = 0

        For i = 0 To 3
            op = i
            curr_voice.Voice.Op(op).AR = Registers(&H50 + chan + (op * 4) + 256 * port) And 31
            curr_voice.Voice.Op(op).D1R = Registers(&H60 + chan + (op * 4) + 256 * port) And 31
            curr_voice.Voice.Op(op).D2R = 0
            curr_voice.Voice.Op(op).SR = Registers(&H70 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).RR = Registers(&H80 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).D1L = 0
            curr_voice.Voice.Op(op).TL = Registers(&H40 + chan + (op * 4) + 256 * port) And 127
            curr_voice.Voice.Op(op).KS = (Registers(&H50 + chan + (op * 4) + 256 * port) >> 6) And 3
            curr_voice.Voice.Op(op).MUL = Registers(&H30 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).DT1 = (Registers(&H30 + chan + (op * 4) + 256 * port) >> 4) And 7
            curr_voice.Voice.Op(op).DT2 = 0
            curr_voice.Voice.Op(op).SSGEG = Registers(&H90 + chan + (op * 8) + 256 * port) And 15
            curr_voice.Voice.Op(op).AME = 0
        Next

        For i = 1 To 2
            op = 3 - i
            curr_voice.Voice.Op(op).AR = Registers(&H50 + chan + (op * 4) + 256 * port) And 31
            curr_voice.Voice.Op(op).D1R = Registers(&H60 + chan + (op * 4) + 256 * port) And 31
            curr_voice.Voice.Op(op).D2R = 0
            curr_voice.Voice.Op(op).SR = Registers(&H70 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).RR = Registers(&H80 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).D1L = 0
            curr_voice.Voice.Op(op).TL = Registers(&H40 + chan + (op * 4) + 256 * port) And 127
            curr_voice.Voice.Op(op).KS = (Registers(&H50 + chan + (op * 4) + 256 * port) >> 6) And 3
            curr_voice.Voice.Op(op).MUL = Registers(&H30 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).DT1 = (Registers(&H30 + chan + (op * 4) + 256 * port) >> 4) And 7
            curr_voice.Voice.Op(op).DT2 = 0
            curr_voice.Voice.Op(op).SSGEG = Registers(&H90 + chan + (op * 8) + 256 * port) And 15
            curr_voice.Voice.Op(op).AME = 0
        Next

        'maximise volume
        TL_Min = 255
        'TL_Change_Count = 0
        For op = 0 To 3
            If (Carrier(curr_voice.Voice.CON, op)) Then
                If (TL_Min > curr_voice.Voice.Op(op).TL) Then TL_Min = curr_voice.Voice.Op(op).TL
            End If
        Next

        If (TL_Min <= 127) Then

            For op = 0 To 3
                If (Carrier(curr_voice.Voice.CON, op)) Then
                    curr_voice.Voice.Op(op).TL = curr_voice.Voice.Op(op).TL - TL_Min
                    'TL_Change_Count = TL_Change_Count + 1
                End If
            Next

            curr_voice.VolumeChangeAmount = TL_Min
            'ListBox2.Items.Add("VolumeChangeAmount= " + TL_Min.ToString)

        Else
            curr_voice.VolumeChangeAmount = 0
            ListBox2.Items.Add("Did not find minimum TL")
        End If

        Return (curr_voice)

    End Function

    Private Function GetCurrentVoiceYM2610(chan As Integer, port As Integer) As CurrVoice_Struct

        Dim curr_voice As CurrVoice_Struct
        Dim i, op As Integer
        Dim TL_Min As Integer

        'Dim TL_Change_Count As Integer

        ReDim curr_voice.Voice.Op(3)

        curr_voice.Voice.Name = ""

        curr_voice.Voice.AMS = (Registers(&HB4 + chan + 256 * port) >> 4) And 3
        curr_voice.Voice.PMS = Registers(&HB4 + chan + 256 * port) And 7

        curr_voice.Voice.LFRQ = 0
        curr_voice.Voice.AMD = 0
        curr_voice.Voice.PMD = 0
        curr_voice.Voice.WF = 0
        curr_voice.Voice.NFRQ = 0
        curr_voice.Voice.PAN = 0
        ' FeedBack
        curr_voice.Voice.FL = ((Registers(&HB0 + chan) >> 3) + 256 * port) And 7
        ' Connect
        curr_voice.Voice.CON = (Registers(&HB0 + chan) + 256 * port) And 7
        ' Slot
        curr_voice.Voice.SLOT = ((Registers(&H28) >> 4)) And &HF
        curr_voice.Voice.NE = 0

        For i = 0 To 3
            op = i
            curr_voice.Voice.Op(op).AR = Registers(&H50 + chan + (op * 4) + 256 * port) And 31
            curr_voice.Voice.Op(op).D1R = Registers(&H60 + chan + (op * 4) + 256 * port) And 31
            curr_voice.Voice.Op(op).D2R = 0
            curr_voice.Voice.Op(op).SR = Registers(&H70 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).RR = Registers(&H80 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).D1L = 0
            curr_voice.Voice.Op(op).TL = Registers(&H40 + chan + (op * 4) + 256 * port) And 127
            curr_voice.Voice.Op(op).KS = (Registers(&H50 + chan + (op * 4) + 256 * port) >> 6) And 3
            curr_voice.Voice.Op(op).MUL = Registers(&H30 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).DT1 = (Registers(&H30 + chan + (op * 4) + 256 * port) >> 4) And 7
            curr_voice.Voice.Op(op).DT2 = 0
            curr_voice.Voice.Op(op).SSGEG = Registers(&H90 + chan + (op * 8) + 256 * port) And 15
            curr_voice.Voice.Op(op).AME = 0
        Next

        For i = 1 To 2
            op = 3 - i
            curr_voice.Voice.Op(op).AR = Registers(&H50 + chan + (op * 4) + 256 * port) And 31
            curr_voice.Voice.Op(op).D1R = Registers(&H60 + chan + (op * 4) + 256 * port) And 31
            curr_voice.Voice.Op(op).D2R = 0
            curr_voice.Voice.Op(op).SR = Registers(&H70 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).RR = Registers(&H80 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).D1L = 0
            curr_voice.Voice.Op(op).TL = Registers(&H40 + chan + (op * 4) + 256 * port) And 127
            curr_voice.Voice.Op(op).KS = (Registers(&H50 + chan + (op * 4) + 256 * port) >> 6) And 3
            curr_voice.Voice.Op(op).MUL = Registers(&H30 + chan + (op * 4) + 256 * port) And 15
            curr_voice.Voice.Op(op).DT1 = (Registers(&H30 + chan + (op * 4) + 256 * port) >> 4) And 7
            curr_voice.Voice.Op(op).DT2 = 0
            curr_voice.Voice.Op(op).SSGEG = Registers(&H90 + chan + (op * 8) + 256 * port) And 15
            curr_voice.Voice.Op(op).AME = 0
        Next

        'maximise volume
        TL_Min = 255
        'TL_Change_Count = 0
        For op = 0 To 3
            If (Carrier(curr_voice.Voice.CON, op)) Then
                If (TL_Min > curr_voice.Voice.Op(op).TL) Then TL_Min = curr_voice.Voice.Op(op).TL
            End If
        Next

        If (TL_Min <= 127) Then

            For op = 0 To 3
                If (Carrier(curr_voice.Voice.CON, op)) Then
                    curr_voice.Voice.Op(op).TL = curr_voice.Voice.Op(op).TL - TL_Min
                    'TL_Change_Count = TL_Change_Count + 1
                End If
            Next

            curr_voice.VolumeChangeAmount = TL_Min
            'ListBox2.Items.Add("VolumeChangeAmount= " + TL_Min.ToString)

        Else
            curr_voice.VolumeChangeAmount = 0
            ListBox2.Items.Add("Did not find minimum TL")
        End If

        Return (curr_voice)

    End Function

    Private Sub SendYM()

        Dim KF_PB As Integer
        Dim Chan As Integer
        Dim Vol As Double

        Registers(ym_reg) = ym_val

        Select Case (ym_reg)

            ' #08: Key On
            ' 0-2: Channel Number [8 Channels, 0-7]
            ' 3: CAR2
            ' 4: MOD2
            ' 5: CAR1
            ' 6: MOD1
            ' 7: (Not Used)

            Case &H8
                'Key on
                Chan = ym_val And &H7

                Slot(0, Chan) = (ym_val And &H78) >> 3
                NoteOn_Old(0, Chan) = NoteOn(0, Chan)

                If (Slot(0, Chan) <> 0) Then
                    'If (RegisterChanged) Then

                    'capture current instrument
                    VolumeChangeAmount_old(0, Chan) = CurrentVoice(0, Chan).VolumeChangeAmount
                    CurrentVoice(0, Chan) = GetCurrentVoice(Chan)

                    'compare with insturment list and add if its not there, get instrument number
                    VoiceID_old(0, Chan) = VoiceID(0, Chan)
                    VoiceID(0, Chan) = FindVoice(CurrentVoice(0, Chan).Voice)

                    'if instrument number has changed, send program change command
                    If (VoiceID_old(0, Chan) <> VoiceID(0, Chan)) Then
                        Send_Midi(&HC0 + Chan, VoiceID(0, Chan), -1)
                    End If

                    'if volume change amount has changed then send volume command
                    If (VolumeChangeAmount_old(0, Chan) <> CurrentVoice(0, Chan).VolumeChangeAmount) Then

                        Vol = -(CurrentVoice(0, Chan).VolumeChangeAmount * 0.75)
                        Vol = (10 ^ (Vol / 40)) * 127
                        Vol = Vol * Gain

                        If (Vol > MaxVol) Then MaxVol = Vol

                        If Vol > 127 Then Vol = 127
                        If Vol < 0 Then Vol = 0

                        Send_Midi(&HB0 + Chan, 7, Vol)
                    End If

                    RegisterChanged = False
                    'End If

                    NoteOn(0, Chan) = True
                Else
                    NoteOn(0, Chan) = False
                End If

                If (NoteOn_Old(0, Chan) <> NoteOn(0, Chan)) Then
                    If (NoteOn(0, Chan)) Then
                        If (Note(0, Chan) >= 0) Then
                            Send_Midi(&H90 + Chan, Note(0, Chan), 127)
                        Else
                            ListBox2.Items.Add("Key on occured before note was set!")
                        End If
                    Else
                        If (Note(0, Chan) >= 0) Then Send_Midi(&H80 + Chan, Note(0, Chan), 0)
                    End If
                End If

            Case &H28 To &H2F
                'Key code, 3 bits octave and 4 bits note
                Chan = ym_reg And &H7
                Note_Old(0, Chan) = Note(0, Chan)
                Note(0, Chan) = KeyCodeToMIDINote(ym_val)

                If (NoteOn(0, Chan) And (Note(0, Chan) <> Note_Old(0, Chan))) Then
                    If (Note_Old(0, Chan) >= 0) Then Send_Midi(&H80 + Chan, Note_Old(0, Chan), 0)
                    Send_Midi(&H90 + Chan, Note(0, Chan), 127)
                End If

            Case &H30 To &H37
                'Key fraction
                Chan = ym_reg And &H7

                KF(Chan) = ym_val >> 2

                If (KF(Chan) <> KF_old(Chan)) Then
                    'ListBox4.Items.Add("Key fraction chan= " + Chan.ToString + " val= " + KF(Chan).ToString)
                    KF_old(Chan) = KF(Chan)

                    'Pitch bend range should be 2 semitones

                    KF_PB = KF(Chan) * 64
                    KF_PB = KF_PB + 8192

                    Send_Midi(&HE0 + Chan, KF_PB And &H7F, KF_PB >> 7)
                End If

            Case &H19
                'Phase or amplitude modulation depth (AMD / PMD)

                If ((ym_val And &H80) = 0) Then
                    'AMD
                    AMD = ym_val And 127
                Else
                    'PMD
                    PMD = ym_val And 127
                End If

                RegisterChanged = True

            Case &H18, &H1B, &H20 To &HFF
                RegisterChanged = True

        End Select

    End Sub

    '''''' TODO: Add MIDI Control Events ''''''
    Private Sub SendYM2203(ym_chip As Integer, next_reg As Integer, pre_fetch As Boolean)
        Dim Chan As Integer
        Dim Vol As Double

        Registers(ym_reg + ym_chip * 256) = ym_val

        Select Case (ym_reg)

            ' ****** SSG Part ******
            ' Set Fine Tune 
            Case &H0, &H2, &H4
                ' SSG Channel
                Chan = (ym_reg >> 1) And &H3
                SSGTuneFine(ym_chip, Chan) = ym_val And &HFF

                SSGNote_Old(ym_chip, Chan) = SSGNote(ym_chip, Chan)
                SSGNote(ym_chip, Chan) = KeyCodeToMIDINoteSSG(ym_chip, Chan, VGMFile.iFreqChip(chipTag.SND_YM2203))

                If ((pre_fetch And (next_reg <> &H1) And (next_reg <> &H3) And (next_reg <> &H5)) Or pre_fetch <> True) Then
                    If (SSGNoteOn(ym_chip, Chan) And (SSGNote(ym_chip, Chan) <> SSGNote_Old(ym_chip, Chan))) Then
                        If (SSGNote_Old(ym_chip, Chan) >= 0) Then
                            Send_Midi(&H80 + Chan + 6 * ym_chip, SSGNote_Old(ym_chip, Chan), 0)
                        End If
                        Send_Midi(&H90 + Chan + 6 * ym_chip, SSGNote(ym_chip, Chan), 127)
                        ListBox2.Items.Add("0x90 Event Occured In Channel " & (Chan + 3 * ym_chip) & " Next Reg: " & next_reg)
                    End If

                    If (PitchWheel(ym_chip, Chan) <> LastPitchWheel(ym_chip, Chan)) Then
                        Send_Midi(&HE0 + Chan + 6 * ym_chip, PitchWheel(ym_chip, Chan) And &H7F, (PitchWheel(ym_chip, Chan) >> 7) And &H7F)
                        LastPitchWheel(ym_chip, Chan) = PitchWheel(ym_chip, Chan)
                        ListBox2.Items.Add("0xE0 Event Occured In Channel " & (Chan + 3 * ym_chip) & " Next Reg: " & next_reg)
                    End If
                End If

            ' Set Coarse Tune
            Case &H1, &H3, &H5
                ' SSG Channel
                Chan = (ym_reg >> 1) And &H3
                SSGTuneCoarse(ym_chip, Chan) = ym_val And &HF

                SSGNote_Old(ym_chip, Chan) = SSGNote(ym_chip, Chan)
                SSGNote(ym_chip, Chan) = KeyCodeToMIDINoteSSG(ym_chip, Chan, VGMFile.iFreqChip(chipTag.SND_YM2203))

                If ((pre_fetch And (next_reg <> &H0) And (next_reg <> &H2) And (next_reg <> &H4)) Or pre_fetch <> True) Then
                    If (SSGNoteOn(ym_chip, Chan) And (SSGNote(ym_chip, Chan) <> SSGNote_Old(ym_chip, Chan))) Then
                        If (SSGNote_Old(ym_chip, Chan) >= 0) Then
                            Send_Midi(&H80 + Chan + 6 * ym_chip, SSGNote_Old(ym_chip, Chan), 0)
                        End If
                        Send_Midi(&H90 + Chan + 6 * ym_chip, SSGNote(ym_chip, Chan), 127)
                        ListBox2.Items.Add("0x90 Event Occured In Channel " & Chan & " Next Reg: " & next_reg)
                    End If

                    If (PitchWheel(ym_chip, Chan) <> LastPitchWheel(ym_chip, Chan)) Then
                        Send_Midi(&HE0 + Chan + 6 * ym_chip, PitchWheel(ym_chip, Chan) And &H7F, (PitchWheel(ym_chip, Chan) >> 7) And &H7F)
                        LastPitchWheel(ym_chip, Chan) = PitchWheel(ym_chip, Chan)
                        ListBox2.Items.Add("0xE0 Event Occured In Channel " & Chan & " Next Reg: " & next_reg)
                    End If
                End If

            Case &H7
                If (ym_val And &H1) Then
                    SSGMix(ym_chip, 0) = False
                Else
                    SSGMix(ym_chip, 0) = True
                End If

                If (ym_val And &H2) Then
                    SSGMix(ym_chip, 1) = False
                Else
                    SSGMix(ym_chip, 1) = True
                End If

                If (ym_val And &H4) Then
                    SSGMix(ym_chip, 2) = False
                Else
                    SSGMix(ym_chip, 2) = True
                End If

            ' Set Volume
            Case &H8 To &HA
                Chan = (ym_reg) And &H3
                SSGVolume_Old(ym_chip, Chan) = SSGVolume(ym_chip, Chan)
                SSGVolume(ym_chip, Chan) = ((ym_val And &HF) << 1) + 1

                SSGNoteOn_Old(ym_chip, Chan) = SSGNoteOn(ym_chip, Chan)

                If (SSGMix(ym_chip, Chan)) Then
                    'If (OnceFlag(Chan) <> True) Then
                    '    Send_Midi(&HC0 + Chan, 4, -1)
                    '    OnceFlag(Chan) = True
                    'End If

                    If (SSGVolume_Old(ym_chip, Chan) <> SSGVolume(ym_chip, Chan)) Then
                        Send_Midi(&HB0 + Chan + 6 * ym_chip, 7, 256 * VolumeTable_YM2149(SSGVolume(ym_chip, Chan)) / 256.0)
                        ListBox2.Items.Add("0xB0 Event Occured In Channel " & Chan & " Next Reg: " & next_reg)
                    End If

                    RegisterChanged = False

                    SSGNoteOn(ym_chip, Chan) = True
                Else
                    SSGNoteOn(ym_chip, Chan) = False
                End If

                'If (SSGNoteOn_Old(Chan) <> SSGNoteOn(Chan)) Then
                '    If (SSGNoteOn(Chan)) Then
                '        If (SSGNote(Chan) >= 0) Then
                '            Send_Midi(&H90 + Chan, SSGNote(Chan), 127)
                '            ListBox2.Items.Add("0x90 Event Occured In Channel " & Chan & " Next Reg: " & next_reg)
                '        Else
                '            ListBox2.Items.Add("Key on occured before note was set!")
                '        End If
                '    Else
                '        If (SSGNote(Chan) >= 0) Then Send_Midi(&H80 + Chan, SSGNote(Chan), 0)
                '    End If
                'End If

            ' ****** FM Part ******
            Case &H28
                Chan = ym_val And &H3

                Slot(ym_chip, Chan) = (ym_val And &HFF) >> 4
                NoteOn_Old(ym_chip, Chan) = NoteOn(ym_chip, Chan)

                If (Slot(ym_chip, Chan) <> 0) Then
                    ' Instrument Comparision

                    'capture current instrument
                    VolumeChangeAmount_old(ym_chip, Chan) = CurrentVoice(ym_chip, Chan).VolumeChangeAmount
                    CurrentVoice(ym_chip, Chan) = GetCurrentVoiceYM2203(ym_chip, Chan)

                    'compare with instrument list and add if its not there, get instrument number
                    VoiceID_old(ym_chip, Chan) = VoiceID(ym_chip, Chan)
                    VoiceID(ym_chip, Chan) = FindVoice(CurrentVoice(ym_chip, Chan).Voice)

                    If (VoiceID_old(ym_chip, Chan) <> VoiceID(ym_chip, Chan)) Then
                        Send_Midi(&HC0 + Chan + 3 + 6 * ym_chip, VoiceID(ym_chip, Chan), -1)
                    End If

                    'if volume change amount has changed then send volume command
                    If (VolumeChangeAmount_old(ym_chip, Chan) <> CurrentVoice(ym_chip, Chan).VolumeChangeAmount) Then

                        Vol = -(CurrentVoice(ym_chip, Chan).VolumeChangeAmount * 0.75)
                        Vol = (10 ^ (Vol / 40)) * 127
                        Vol = Vol * Gain

                        If (Vol > MaxVol) Then MaxVol = Vol

                        If Vol > 127 Then Vol = 127
                        If Vol < 0 Then Vol = 0

                        ' Volume Meta Event
                        Send_Midi(&HB0 + Chan + 3 + 6 * ym_chip, 7, Vol)
                    End If

                    RegisterChanged = False
                    'End If

                    NoteOn(ym_chip, Chan) = True
                Else
                    NoteOn(ym_chip, Chan) = False
                End If

                If (NoteOn_Old(ym_chip, Chan) <> NoteOn(ym_chip, Chan)) Then
                    If (NoteOn(ym_chip, Chan)) Then
                        If (Note(ym_chip, Chan) >= 0) Then
                            Send_Midi(&H90 + Chan + 3 + 6 * ym_chip, Note(ym_chip, Chan), 127)
                        Else
                            ListBox2.Items.Add("Key on occured before note was set!")
                        End If
                    Else
                        If (Note(ym_chip, Chan) >= 0) Then Send_Midi(&H80 + Chan + 3 + 6 * ym_chip, Note(ym_chip, Chan), 0)
                    End If
                End If

            ' Voice Registers
            Case &H30 To &H9F, &HB0 To &HB2
                RegisterChanged = True

            ' F-Number Low
            ' Play Note
            Case &HA0 To &HA2
                Chan = ym_reg And &H3
                FNumLow(ym_chip, Chan) = ym_val

                Note_Old(ym_chip, Chan) = Note(ym_chip, Chan)
                Note(ym_chip, Chan) = KeyCodeToMIDINoteYM2203(ym_chip, Chan)

                If (NoteOn(ym_chip, Chan) And (Note(ym_chip, Chan) <> Note_Old(ym_chip, Chan))) Then
                    If (Note_Old(ym_chip, Chan) >= 0) Then Send_Midi(&H80 + Chan + 3 + 6 * ym_chip, Note_Old(ym_chip, Chan), 0)
                    Send_Midi(&H90 + Chan + 3 + 6 * ym_chip, Note(ym_chip, Chan), 127)
                End If

            ' F-Number High and Octave Block
            Case &HA4 To &HA6
                Chan = ym_reg And &H3
                FNumHigh(ym_chip, Chan) = ym_val

        End Select

    End Sub

    Private Sub SendYM2608(ym_port As Integer)

        ' ym_port <= [A1]
        ' 1) A1 = 0
        ' + 00-2F: Addressing of SSG, Commonness Part of FM, and Rhythm
        ' + 30-B6: Addressing of FM Channel 1-3
        ' 1) A1 = 1
        ' + 00-10: Addressing Related To ADPCM
        ' + 30-B6: Addressing of FM Channel 4-6

        Dim Chan, ym_slot, ym_chip As Integer
        Dim Vol As Double

        Registers(ym_reg + 256 * ym_port) = ym_val

        Select Case (ym_reg)

            ' ym_port = ?
            Case &H28
                'Key on
                ym_slot = ym_val And &H7
                Chan = ym_val And &H3
                ym_chip = (ym_val >> 2) And &H1

                Slot(0, ym_slot) = (ym_val And &HFF) >> 4
                NoteOn_Old(0, ym_slot) = NoteOn(0, ym_slot)

                'If (Slot(0, Chan) <> 0) Then
                '    'If (RegisterChanged) Then

                '    'capture current instrument
                '    VolumeChangeAmount_old(0, Chan) = CurrentVoice(0, Chan).VolumeChangeAmount
                '    CurrentVoice(0, Chan) = GetCurrentVoiceYM2608(Chan And &H3, (Chan >> 2) And &H1)

                '    'compare with insturment list and add if its not there, get instrument number
                '    VoiceID_old(0, Chan) = VoiceID(0, Chan)
                '    VoiceID(0, Chan) = FindVoice(CurrentVoice(0, Chan).Voice)

                '    RegisterChanged = False
                '    NoteOn(0, Chan) = True
                'Else
                '    NoteOn(0, Chan) = False
                'End If

                If (Slot(ym_chip, Chan) <> 0) Then
                    ' Instrument Comparision

                    'capture current instrument
                    VolumeChangeAmount_old(ym_chip, Chan) = CurrentVoice(ym_chip, Chan).VolumeChangeAmount
                    CurrentVoice(ym_chip, Chan) = GetCurrentVoiceYM2608(ym_chip, Chan)

                    'compare with instrument list and add if its not there, get instrument number
                    VoiceID_old(ym_chip, Chan) = VoiceID(ym_chip, Chan)
                    VoiceID(ym_chip, Chan) = FindVoice(CurrentVoice(ym_chip, Chan).Voice)

                    If (VoiceID_old(ym_chip, Chan) <> VoiceID(ym_chip, Chan)) Then
                        Send_Midi(&HC0 + ym_slot, VoiceID(ym_chip, Chan), -1)
                    End If

                    'if volume change amount has changed then send volume command
                    If (VolumeChangeAmount_old(ym_chip, Chan) <> CurrentVoice(ym_chip, Chan).VolumeChangeAmount) Then

                        Vol = -(CurrentVoice(ym_chip, Chan).VolumeChangeAmount * 0.75)
                        Vol = (10 ^ (Vol / 40)) * 127
                        Vol = Vol * Gain

                        If (Vol > MaxVol) Then MaxVol = Vol

                        If Vol > 127 Then Vol = 127
                        If Vol < 0 Then Vol = 0

                        ' Volume Meta Event
                        Send_Midi(&HB0 + ym_slot, 7, Vol)
                    End If

                    RegisterChanged = False
                    'End If

                    NoteOn(ym_chip, Chan) = True
                Else
                    NoteOn(ym_chip, Chan) = False
                End If

                If (NoteOn_Old(ym_chip, Chan) <> NoteOn(ym_chip, Chan)) Then
                    If (NoteOn(ym_chip, Chan)) Then
                        If (Note(ym_chip, Chan) >= 0) Then
                            Send_Midi(&H90 + ym_slot, Note(ym_chip, Chan), 127)
                        Else
                            ListBox2.Items.Add("Key on occured before note was set!")
                        End If
                    Else
                        If (Note(ym_chip, Chan) >= 0) Then Send_Midi(&H80 + ym_slot, Note(ym_chip, Chan), 0)
                    End If
                End If

            ' Voice Registers
            Case &H30 To &H9F, &HB0 To &HB6
                RegisterChanged = True

            ' F-Number Low
            ' Play Note
            Case &HA0 To &HA2
                Chan = ym_reg And &H3

                FNumLow(ym_port, Chan) = ym_val

                Note_Old(ym_port, Chan) = Note(ym_port, Chan)
                Note(ym_port, Chan) = KeyCodeToMIDINoteYM2608(ym_port, Chan)

                If (NoteOn(ym_port, Chan) And (Note(ym_port, Chan) <> Note_Old(ym_port, Chan))) Then
                    If (Note_Old(ym_port, Chan) >= 0) Then Send_Midi(&H80 + Chan + 3 * ym_port, Note_Old(ym_port, Chan), 0)
                    Send_Midi(&H90 + Chan + 3 * ym_port, Note(ym_port, Chan), 127)
                End If

            ' F-Number High and Octave Block
            Case &HA4 To &HA6
                Chan = ym_reg And &H3

                FNumHigh(ym_port, Chan) = ym_val

        End Select

    End Sub

    Private Sub SendYM2610(ym_port As Integer)

        ' ym_port <= [A1]
        ' Channel Numbering:
        ' + Channel 1: 001
        ' + Channel 2: 010
        ' + Channel 3: 101
        ' + Channel 4: 110

        Dim Chan As Integer

        Registers(ym_reg + 256 * ym_port) = ym_val

        Select Case (ym_reg)

            Case &H28
                'Key on
                Chan = ym_val And &H7

                Slot(0, Chan) = (ym_val And &HFF) >> 4
                NoteOn_Old(0, Chan) = NoteOn(0, Chan)

                If (Slot(0, Chan) <> 0) Then
                    'If (RegisterChanged) Then

                    'capture current instrument
                    VolumeChangeAmount_old(0, Chan) = CurrentVoice(0, Chan).VolumeChangeAmount
                    CurrentVoice(0, Chan) = GetCurrentVoiceYM2610(Chan And &H3, (Chan >> 2) And &H1)

                    'compare with insturment list and add if its not there, get instrument number
                    VoiceID_old(0, Chan) = VoiceID(0, Chan)
                    VoiceID(0, Chan) = FindVoice(CurrentVoice(0, Chan).Voice)

                    RegisterChanged = False
                    NoteOn(0, Chan) = True
                Else
                    NoteOn(0, Chan) = False
                End If

            ' Voice Registers
            Case &H30 To &H9F, &HB0 To &HB6
                RegisterChanged = True

        End Select

    End Sub

    Private Sub Parse()

        ' Sampling Rate: 44100 Hz
        ' Sample Delay = {Samples} / {Sampling Rate}

        Select Case (d(0))
            ' [Command] [dd] (Write Value {dd})
            ' 0x30 - 0x3F: Used For Dual Chip Support
            ' 0x4F: Game Gear PSG Streo, to Port 0x06
            ' 0x50: PSG (SN76489/SN76496)
            Case &H30 To &H3F, &H4F, &H50
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                in_file.Read(d, 0, 1) : filepos = filepos + 1

            ' [Command] [dd] [aa] (Write Value {dd} to Port {aa})
            ' 0x40 - 0x4E: Reserved For Future Use (Only 1 Operand Before 1.60)
            ' 0x51: YM2413
            ' 0x52: YM2612 Port 0
            ' 0x53: YM2612 Port 1
            ' 0x54: YM2151
            ' 0x55: YM2203
            ' 0x56: YM2608 Port 0
            ' 0x57: YM2608 Port 1 
            ' 0x58: YM2610 Port 0
            ' 0x59: YM2610 Port 1
            ' 0x5A: YM3812
            ' 0x5B: YM3526
            ' 0x5C: Y8950
            ' 0x5D: YMZ280B
            ' 0x5E: YMF262 Port 0
            ' 0x5F: YMF262 Port 1
            ' 0xA0: AY8910
            ' 0xA1 - 0xAF: Reserved For Future Use (Or Dual Chip Support)
            ' 0xB0: RF5C68
            ' 0xB1: RF5C164
            ' 0xB2: PWM ({ddd} to {a})
            ' 0xB3: GB DMG
            ' 0xB4: NES APU
            ' 0xB5: MultiPCM
            ' 0xB6: uPD7759
            ' 0xB7: OKIM6258
            ' 0xB8: OKIM6295
            ' 0xB9: HuC6280
            ' 0xBA: K053260
            ' 0xBB: Pokey
            ' 0xBC: WonderSwan
            ' 0xBD: SAA1099
            ' 0xBE: ES5506
            ' 0xBF: GA20
            Case &H40 To &H4E, &H51 To &H53, &H5B To &H5F, &HA0 To &HA4, &HA6 To &HBF
                in_file.Read(d, 0, 2) : filepos = filepos + 2
                in_file.Read(d, 0, 1) : filepos = filepos + 1

            ' [Command] [pp] [dd] [aa] (Port {pp}, Write Value {dd} to Port {aa})
            ' 0xC9 - 0xCF: Reserved For Future Use
            ' 0xD7 - 0xDF: Reserved For Future Use
            Case &HC0 To &HDF, &H64
                in_file.Read(d, 0, 3) : filepos = filepos + 3
                in_file.Read(d, 0, 1) : filepos = filepos + 1
            Case &HE0 To &HEF
                in_file.Read(d, 0, 4) : filepos = filepos + 4
                in_file.Read(d, 0, 1) : filepos = filepos + 1

            ' ------------------------------------------------------------------------
            ' YM2151 Handling Procedure
            Case &H54
                in_file.Read(d, 0, 2) : filepos = filepos + 2
                'YM2151, write value d(1) to register d(0)

                ' 0x54 [dd] [aa]: Write {dd} to Register {aa}
                ym_reg = d(0)   ' dd
                ym_val = d(1)   ' aa

                ' Fetch Next Instructions (StreamFlow)
                in_file.Read(d, 0, 1) : filepos = filepos + 1

                ' ** Main YM2151 Processing Subroutine
                SendYM()

            ' ------------------------------------------------------------------------
            ' YM2203 Handling Procedure
            Case &H55
                in_file.Read(d, 0, 2) : filepos = filepos + 2

                ' 0x55 [dd] [aa]:  Write {dd} to Register {aa}
                ym_reg = d(0)   ' dd
                ym_val = d(1)   ' aa

                in_file.Read(d, 0, 1) : filepos = filepos + 1

                ' ** Main YM2203 Processing Subroutine
                Dim Temp As Byte = d(0)
                Dim next_reg As Byte = 0
                Dim seek_off As Integer = 0

                ' Pre-Fetch (Same Command)
                While ((d(0) <= &H8F And d(0) >= &H70))
                    in_file.Read(d, 0, 1) : seek_off = seek_off - 1
                End While

                If (d(0) <> &H55) Then
                    SendYM2203(0, next_reg, False) : in_file.Seek(seek_off, SeekOrigin.Current)
                Else
                    in_file.Read(d, 0, 1) : in_file.Seek(seek_off - 1, SeekOrigin.Current)
                    next_reg = d(0)
                    SendYM2203(0, next_reg, True)
                End If

                ' Recover
                d(0) = Temp

            ' Dual YM2203 Support
            Case &HA5
                in_file.Read(d, 0, 2) : filepos = filepos + 2

                ' 0xA5 [dd] [aa]:  Write {dd} to Register {aa}
                ym_reg = d(0)   ' dd
                ym_val = d(1)   ' aa

                in_file.Read(d, 0, 1) : filepos = filepos + 1

                ' ** Main YM2203 Processing Subroutine
                Dim Temp As Byte = d(0)
                Dim next_reg As Byte = 0
                Dim seek_off As Integer = 0

                ' Pre-Fetch (Same Command)
                While ((d(0) <= &H8F And d(0) >= &H70))
                    in_file.Read(d, 0, 1) : seek_off = seek_off - 1
                End While

                If (d(0) <> &HA5) Then
                    SendYM2203(1, next_reg, False) : in_file.Seek(seek_off, SeekOrigin.Current)
                Else
                    in_file.Read(d, 0, 1) : in_file.Seek(seek_off - 1, SeekOrigin.Current)
                    next_reg = d(0)
                    SendYM2203(1, next_reg, True)
                End If

                ' Recover
                d(0) = Temp

            ' ------------------------------------------------------------------------
            Case &H56 To &H57
                Dim ym_port As Integer = d(0) And &H1

                in_file.Read(d, 0, 2) : filepos = filepos + 2
                ' 0x56 [dd] [aa]:  Write {dd} to Register {aa} (Port 0)
                ym_reg = d(0)   ' dd
                ym_val = d(1)   ' aa

                in_file.Read(d, 0, 1) : filepos = filepos + 1

                SendYM2608(ym_port)

            ' YM2610 Port 0/1
            Case &H58, &H59
                Dim ym_port As Integer = d(0) And &H1

                in_file.Read(d, 0, 2) : filepos = filepos + 2
                ' 0x56 [dd] [aa]:  Write {dd} to Register {aa} (Port 0)
                ym_reg = d(0)   ' dd
                ym_val = d(1)   ' aa

                in_file.Read(d, 0, 1) : filepos = filepos + 1

                SendYM2610(ym_port)

            Case &H5A
                in_file.Read(d, 0, 2) : filepos = filepos + 2

                ym_reg = d(0)
                ym_val = d(1)

                ' SendYM3812()

            Case &H61
                in_file.Read(d, 0, 2) : filepos = filepos + 2
                'wait number of samples
                delay = delay + BytesToInt16(d)
                in_file.Read(d, 0, 1) : filepos = filepos + 1
            Case &H62
                'wait 735 samples (1/60 seconds)
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                delay = delay + 735
            Case &H63
                'wait 882 samples (1/50 seconds)
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                delay = delay + 882
            Case &H66
                'end of sound data
                'ListBox1.Items.Add("end of sound data at " + filepos.ToString)
                filepos = filelength
                'file.Read(d, 0, 1) : filepos = filepos + 1
            Case &H67
                'ListBox1.Items.Add("data block at " + filepos.ToString)
                'data block..
                Dim iDataType, iDataLength As Integer
                Dim szFileName As String = RemoveFileExt(szOrigFileName)
                Dim byteData(&H100000) As Byte

                ' 0x66: Compatibility Command to Make Players Stop Parsing The Stream 
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                ' tt: Data Types
                ' 00 - n-Bit Compression
                ' 01 - DPCM Compression
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                iDataType = d(0)

                ' ss ss ss ss - Data Size
                in_file.Read(d, 0, 4) : filepos = filepos + 4
                iDataLength = BytesToInt32(d)

                ' in_file.Seek(filepos, SeekOrigin.Begin)
                in_file.Read(byteData, 0, iDataLength) : filepos = filepos + iDataLength
                If (filepos < 0) Then
                    filepos = filelength
                    'ListBox1.Items.Add("Abort at " + filepos.ToString)
                End If

                WriteDataBlock(iDataType, szFileName, byteData, iDataLength)

                in_file.Read(d, 0, 1) : filepos = filepos + 1
            Case &H68
                'PCM RAM write...
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                in_file.Read(d, 0, 3) : filepos = filepos + 3
                in_file.Read(d, 0, 3) : filepos = filepos + 3
                in_file.Read(d, 0, 3) : filepos = filepos + 3

                in_file.Read(d, 0, 1) : filepos = filepos + 1
            Case &H70 To &H8F
                'wait number of samples (use lower nibble of command)
                delay = delay + (d(0) And 15)
                in_file.Read(d, 0, 1) : filepos = filepos + 1
            Case &H90, &H91, &H95
                'DAC stream control...
                in_file.Read(d, 0, 4) : filepos = filepos + 4
                in_file.Read(d, 0, 1) : filepos = filepos + 1
            Case &H92
                'DAC stream control...
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                in_file.Read(d, 0, 4) : filepos = filepos + 4
                in_file.Read(d, 0, 1) : filepos = filepos + 1
            Case &H93
                'DAC stream control...
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                in_file.Read(d, 0, 4) : filepos = filepos + 4
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                in_file.Read(d, 0, 4) : filepos = filepos + 4
                in_file.Read(d, 0, 1) : filepos = filepos + 1
            Case &H94
                'DAC stream control...
                in_file.Read(d, 0, 1) : filepos = filepos + 1
                in_file.Read(d, 0, 1) : filepos = filepos + 1
        End Select
    End Sub

    Private Sub ParseLoop()

        Dim a As Integer

        'Do While ((filepos < filelength) And (abort = False))
        Do While (filepos < filelength)
            a = 0
            Do While ((filepos < filelength) And (a < 5))
                Parse()
                a = a + 1
            Loop
            'Application.DoEvents()
        Loop

        in_file.Close()

    End Sub

    Private Function Checksum(data() As Byte) As Byte

        Dim check As Integer
        Dim check_frlp As Integer

        check = 0
        For check_frlp = 0 To data.Length - 1
            check = check + data(check_frlp)
        Next
        check = ((Not check) + 1) And 127

        Return check

    End Function

    Private Function Voice_to_FB01(Voice As Voice_Struct) As Byte()

        Dim fb01_voice(63) As Byte
        Dim frlp As Integer
        Dim TL As Integer
        Dim op2 As Integer
        Dim car As Integer

        fb01_voice = {83, 105, 110, 101, 87, 97, 118, 0, 205, 128, 0, 120, 0, 0, 64, 0,
              127, 0, 0, 1, 31, 128, 0, 15, 127, 0, 0, 1, 31, 128, 0, 15,
              127, 0, 0, 1, 31, 128, 0, 15, 0, 0, 0, 1, 31, 128, 0, 15,
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If (Voice.Name <> "") Then
            For frlp = 0 To 6
                If (frlp < Voice.Name.Length) Then
                    fb01_voice(frlp) = Asc(Mid(Voice.Name, frlp + 1, 1))
                Else
                    fb01_voice(frlp) = Asc(" ")
                End If
            Next

            fb01_voice(7) = 0     'Users code
            fb01_voice(8) = Voice.LFRQ And 255
            fb01_voice(9) = Voice.AMD And 127
            fb01_voice(10) = Voice.PMD And 127
            fb01_voice(11) = Voice.SLOT And 120
            fb01_voice(12) = ((Voice.FL And 7) << 3) + (Voice.CON And 7)
            fb01_voice(13) = (Voice.AMS And 3) + ((Voice.PMS And 7) << 4)
            fb01_voice(14) = (Voice.WF And 3) << 5
            fb01_voice(15) = 0    'Transpose

            For frlp = 0 To 3
                If (Carrier((Voice.CON And 7), frlp)) Then
                    car = 1
                Else
                    car = 0
                End If

                TL = Voice.Op(frlp).TL
                If (car = 0) Then TL = TL - 8
                If (TL < 0) Then
                    ListBox2.Items.Add("TL was less than 0")
                    TL = 0
                End If

                Select Case (frlp)
                    Case 0 : op2 = 0
                    Case 1 : op2 = 2
                    Case 2 : op2 = 1
                    Case 3 : op2 = 3
                End Select

                fb01_voice(op2 * 8 + 16) = TL And 127
                fb01_voice(op2 * 8 + 17) = 0      'KStype, Velocity sensitivity
                fb01_voice(op2 * 8 + 18) = 0      'Level Scaling, TL adjust
                fb01_voice(op2 * 8 + 19) = (Voice.Op(frlp).MUL And 15) + ((Voice.Op(frlp).DT1 And 7) << 4)
                fb01_voice(op2 * 8 + 20) = (Voice.Op(frlp).AR And 31) + ((Voice.Op(frlp).KS And 3) << 6)
                'fb01_voice(op2 * 8 + 21) = (Voice.Op(frlp).AME And 128) + (Voice.Op(frlp).D1R And 31)
                fb01_voice(op2 * 8 + 21) = (car << 7) + (Voice.Op(frlp).D1R And 31)
                fb01_voice(op2 * 8 + 22) = (Voice.Op(frlp).D2R And 31) + ((Voice.Op(frlp).DT2 And 3) << 6)
                fb01_voice(op2 * 8 + 23) = (Voice.Op(frlp).RR And 15) + ((Voice.Op(frlp).D1L And 15) << 4)
            Next

            For frlp = 48 To 57
                fb01_voice(frlp) = 0       'Reserved
            Next

            fb01_voice(58) = 0      'Poly / mono, Portamento speed
            fb01_voice(59) = 2      'Input controller (PMD), Pitch bender range
            fb01_voice(60) = 0      'Reserved
            fb01_voice(61) = 0      'Reserved
            fb01_voice(62) = 0      'Reserved
            fb01_voice(63) = 0      'Reserved
        End If

        Return fb01_voice

    End Function

    Private Sub WriteInsts()

        Dim syx_buff() As Byte
        Dim length As Integer
        Dim fb01_voice() As Byte
        Dim op2 As Integer
        Dim st As String

        out_file_syx.WriteByte(&HF0)
        out_file_syx.WriteByte(&H43)
        out_file_syx.WriteByte(&H75)
        out_file_syx.WriteByte(0)       'System number
        out_file_syx.WriteByte(0)       'Message number
        out_file_syx.WriteByte(0)       'Operation number
        out_file_syx.WriteByte(0)       'Bank number

        ReDim syx_buff(63)
        Array.Clear(syx_buff, 0, syx_buff.Length)
        syx_buff(0) = Asc("O") And &HF
        syx_buff(1) = (Asc("O") And &HF0) >> 4
        syx_buff(2) = Asc("P") And &HF
        syx_buff(3) = (Asc("P") And &HF0) >> 4
        syx_buff(4) = Asc("M") And &HF
        syx_buff(5) = (Asc("M") And &HF0) >> 4
        syx_buff(6) = Asc(" ") And &HF
        syx_buff(7) = (Asc(" ") And &HF0) >> 4
        syx_buff(8) = Asc("B") And &HF
        syx_buff(9) = (Asc("B") And &HF0) >> 4
        syx_buff(10) = Asc("a") And &HF
        syx_buff(11) = (Asc("a") And &HF0) >> 4
        syx_buff(12) = Asc("n") And &HF
        syx_buff(13) = (Asc("n") And &HF0) >> 4
        syx_buff(14) = Asc("k") And &HF
        syx_buff(15) = (Asc("k") And &HF0) >> 4

        length = syx_buff.Length
        out_file_syx.WriteByte((length And &HFF00) >> 8)   'Size (High byte)
        out_file_syx.WriteByte(length And &HFF)            'Size (Low byte)
        out_file_syx.Write(syx_buff, 0, syx_buff.Length)
        out_file_syx.WriteByte(Checksum(syx_buff))

        out_file_opm.WriteLine("//Created by VGM_to_MID by W.J.Holt")
        out_file_opm.WriteLine()

        ReDim syx_buff(127)

        For frlp = 0 To 47

            If (frlp < VoicesCount) Then
                Voices(frlp).Name = "Inst " + frlp.ToString
                fb01_voice = Voice_to_FB01(Voices(frlp))
                out_file_opm.WriteLine("@:" + frlp.ToString + " " + "Inst_" + frlp.ToString)
                out_file_opm.WriteLine("//  LFRQ AMD PMD WF NFRQ")
                out_file_opm.WriteLine("LFO: " + Voices(frlp).LFRQ.ToString + "  " +
                                       Voices(frlp).AMD.ToString + "  " +
                                       Voices(frlp).PMD.ToString + "  " +
                                       Voices(frlp).WF.ToString + "  " +
                                       Voices(frlp).NFRQ.ToString)
                out_file_opm.WriteLine("// PAN FL CON AMS PMS SLOT NE")
                out_file_opm.WriteLine("CH: " + "64" + "  " +
                                       Voices(frlp).FL.ToString + "  " +
                                       Voices(frlp).CON.ToString + "  " +
                                       Voices(frlp).AMS.ToString + "  " +
                                       Voices(frlp).PMS.ToString + "  " +
                                       Voices(frlp).SLOT.ToString + "  " +
                                       Voices(frlp).NE.ToString)
                out_file_opm.WriteLine("//  AR D1R D2R SR RR D1L  TL KS MUL DT1 DT2 SSG-EG AMS-EN")

                For frlp2 = 0 To 3
                    st = ""
                    Select Case (frlp2)
                        Case 0
                            op2 = 0
                            st = "M1: "
                        Case 1
                            op2 = 2
                            st = "C1: "
                        Case 2
                            op2 = 1
                            st = "M2: "
                        Case 3
                            op2 = 3
                            st = "C2: "
                    End Select

                    out_file_opm.WriteLine(st + Voices(frlp).Op(op2).AR.ToString + "  " +
                                       Voices(frlp).Op(op2).D1R.ToString + "  " +
                                       Voices(frlp).Op(op2).D2R.ToString + "  " +
                                       Voices(frlp).Op(op2).SR.ToString + "  " +
                                       Voices(frlp).Op(op2).RR.ToString + "  " +
                                       Voices(frlp).Op(op2).D1L.ToString + "  " +
                                       Voices(frlp).Op(op2).TL.ToString + "  " +
                                       Voices(frlp).Op(op2).KS.ToString + "  " +
                                       Voices(frlp).Op(op2).MUL.ToString + "  " +
                                       Voices(frlp).Op(op2).DT1.ToString + "  " +
                                       Voices(frlp).Op(op2).DT2.ToString + "  " +
                                       Voices(frlp).Op(op2).SSGEG.ToString + "  " +
                                       Voices(frlp).Op(op2).AME.ToString)

                Next frlp2

                out_file_opm.WriteLine()

            Else
                fb01_voice = Voice_to_FB01(Voices(VoicesCount - 1))
            End If

            For frlp2 = 0 To 63
                syx_buff(frlp2 * 2) = fb01_voice(frlp2) And &HF
                syx_buff((frlp2 * 2) + 1) = (fb01_voice(frlp2) And &HF0) >> 4
            Next

            length = syx_buff.Length * 2
            out_file_syx.WriteByte((length And &HFF00) >> 8)   'Size (High byte)
            out_file_syx.WriteByte(length And &HFF)            'Size (Low byte)
            out_file_syx.Write(syx_buff, 0, syx_buff.Length)
            out_file_syx.WriteByte(Checksum(syx_buff))

        Next

        out_file_syx.WriteByte(&HF7)

    End Sub

    Private Sub WriteMIDIHeader()

        Dim buff() As Byte
        ReDim buff(13)

        ' Rewind
        RewindFileStream(out_file_midi, 0)

        '' out_file_midi.Seek(0, SeekOrigin.Begin)

        buff(0) = Asc("M")
        buff(1) = Asc("T")
        buff(2) = Asc("h")
        buff(3) = Asc("d")
        buff(4) = 0
        buff(5) = 0
        buff(6) = 0
        buff(7) = 6                 'MThd chunk length is always 0006
        buff(8) = 0
        buff(9) = 1                 'Format 1
        buff(10) = 0
        buff(11) = 1                '1 Track
        buff(12) = (TQN >> 8) And &HFF
        buff(13) = TQN And &HFF     'ticks per quater note

        ' Default:
        ' TQN: 22050 (22050 Ticks Per Quarter Note)
        ' (Note: Tick is the minimum unit of clocking, instead of microsecond or second, minute...)

        out_file_midi.Write(buff, 0, buff.Length)

        ReDim buff(7)

        buff(0) = Asc("M")
        buff(1) = Asc("T")
        buff(2) = Asc("r")
        buff(3) = Asc("k")
        buff(4) = (MIDIByteCount >> 24) And &HFF
        buff(5) = (MIDIByteCount >> 16) And &HFF
        buff(6) = (MIDIByteCount >> 8) And &HFF
        buff(7) = MIDIByteCount And &HFF

        out_file_midi.Write(buff, 0, buff.Length)

    End Sub

    Private Function Carrier(alg As Integer, op As Integer) As Boolean

        Dim c As Boolean

        c = False

        Select Case (op)
            Case 0  'M1
                If (alg = 7) Then
                    c = True
                End If
            Case 2  'C1
                If (alg >= 4 And alg <= 7) Then
                    c = True
                End If
            Case 1  'M2
                If (alg >= 5 And alg <= 7) Then
                    c = True
                End If
            Case 3  'C2
                c = True
        End Select

        Return c

    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub ParseGD3()
        Dim dwBuf(3) As Byte
        Dim addr As Integer

        ' Obtain The Value of GD3 Offset
        in_file.Seek(ADDR_GD3OFFSET, SeekOrigin.Begin)
        in_file.Read(dwBuf, 0, 4)
        addr = ADDR_GD3OFFSET + BytesToInt32(dwBuf)

        in_file.Seek(addr, SeekOrigin.Begin)

        in_file.Read(dwBuf, 0, 4)
        If (BytesToText(dwBuf, 4) <> "Gd3 ") Then Exit Sub

        ' Get 
        in_file.Read(dwBuf, 0, 4)
        If (&H100 <> BytesToInt32(dwBuf)) Then Exit Sub
        GD3Tag.verGD3 = "1.00"

        ' Get GD3 Tag Length
        in_file.Read(dwBuf, 0, 4)
        GD3Tag.lenGD3 = BytesToInt32(dwBuf)

        GD3Tag.enTrkName = ReadGD3String() : GD3Tag.jpTrkName = ReadGD3String()
        GD3Tag.enGameName = ReadGD3String() : GD3Tag.jpGameName = ReadGD3String()
        GD3Tag.enSysName = ReadGD3String() : GD3Tag.jpSysName = ReadGD3String()
        GD3Tag.enAuthor = ReadGD3String() : GD3Tag.jpAuthor = ReadGD3String()
        GD3Tag.rlsDate = ReadGD3String()
        GD3Tag.vgmMaker = ReadGD3String()
        GD3Tag.strNotes = ReadGD3String()

        PrintGD3()

    End Sub

    Private Sub PrintGD3()
        ListBox1.Items.Add("------------GD3 Info------------")

        ListBox1.Items.Add("GD3 Tag Version: " + GD3Tag.verGD3)

        ListBox1.Items.Add("Track Name (ENG): " + GD3Tag.enTrkName)
        ListBox1.Items.Add("Track Name (JPN): " + GD3Tag.jpTrkName)

        ListBox1.Items.Add("Game Name (ENG): " + GD3Tag.enGameName)
        ListBox1.Items.Add("Game Name (JPN): " + GD3Tag.jpGameName)

        ListBox1.Items.Add("System Name (ENG): " + GD3Tag.enSysName)
        ListBox1.Items.Add("System Name (JPN): " + GD3Tag.jpSysName)

        ListBox1.Items.Add("Author (ENG): " + GD3Tag.enAuthor)
        ListBox1.Items.Add("Author (JPN): " + GD3Tag.jpAuthor)

        ListBox1.Items.Add("Release Date: " + GD3Tag.rlsDate)
        ListBox1.Items.Add("VGM Pack Maker: " + GD3Tag.vgmMaker)
        ListBox1.Items.Add("Additional Notes: " + GD3Tag.strNotes)
    End Sub

    Private Function ReadGD3String() As String
        Dim wchrBuf(1) As Byte

        Dim strTag(1023) As Byte
        Dim lenTag As Integer

        lenTag = 0
        While (True)
            in_file.Read(wchrBuf, 0, 2)
            If (wchrBuf(0) = 0 And wchrBuf(1) = 0) Then Exit While

            strTag(lenTag) = wchrBuf(0) : lenTag = lenTag + 1
            strTag(lenTag) = wchrBuf(1) : lenTag = lenTag + 1

        End While

        Return System.Text.Encoding.Unicode.GetString(strTag)
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' Fill VGM Struct
    Private Sub ScanVGM()
        Dim iBuf As Integer

        ' Init
        VGMFile.iCntChip = 64
        VGMFile.szSndChip = ""

        VGMFile.lenVGM = ReadDwordAddr(in_file, ADDR_FILESIZE)
        VGMFile.verVGM = ReadDwordAddr(in_file, ADDR_VGMVER)

        If (VGMFile.verVGM <= &H150) Then
            VGMFile.iCntChip = 16
        ElseIf (VGMFile.verVGM <= &H160) Then
            VGMFile.iCntChip = 32
        ElseIf (VGMFile.verVGM <= &H170) Then
            VGMFile.iCntChip = 48
        End If

        iBuf = ReadDwordAddr(in_file, ADDR_DATAOFFSET)
        If (VGMFile.verVGM < &H150) Then
            VGMFile.iDataOffset = &H40
        Else
            VGMFile.iDataOffset = iBuf + ADDR_DATAOFFSET
        End If

        ' Redefine The Length of SndChip Array
        ReDim VGMFile.iFreqChip(VGMFile.iCntChip)
        ReDim VGMFile.fActiveChip(VGMFile.iCntChip)
        ReDim VGMFile.fDualChip(VGMFile.iCntChip)

        RewindFileStream(in_file, &H0)
        For indChip As Integer = 0 To VGMFile.iCntChip - 1
            iBuf = ReadDword(in_file)

            ' Init
            VGMFile.fActiveChip(indChip) = False
            VGMFile.fDualChip(indChip) = False

            If (chipMappingCheck(indChip)) Then

                If ((iBuf And (1 << 30)) <> 0) Then
                    VGMFile.fDualChip(indChip) = True
                    iBuf = (iBuf - (1 << 30)) << 1
                End If
                VGMFile.iFreqChip(indChip) = iBuf

                If (iBuf <> 0) Then
                    If (VGMFile.fDualChip(indChip)) Then VGMFile.szSndChip += "2*"
                    VGMFile.szSndChip += chipMapping(indChip) + " "
                    VGMFile.fActiveChip(indChip) = True
                End If
            End If
        Next

        ''''' Debug Output ''''''
        PrintSndChip()

    End Sub

    Private Sub PrintSndChip()
        ListBox1.Items.Add("------------SoundChip-----------")
        ListBox1.Items.Add("SoundChips: " + VGMFile.szSndChip)

        For indChip As Integer = 0 To VGMFile.iCntChip - 1
            If (VGMFile.fActiveChip(indChip)) Then
                ListBox1.Items.Add(chipMapping(indChip) + " Frequency: " + VGMFile.iFreqChip(indChip).ToString + " Hz")
            End If
        Next
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        ' [Local Variables]
        Dim b(1) As Byte
        Dim filename As String
        Dim pathname As String
        Dim frlp As Integer
        Dim temp As Double
        Dim BPM_Period As Integer

        Dim hFile, hTmp As FileStream
        Dim hGZFile As GZipStream

        ' Clear The Status List
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()

        'we need to check if file is already open
        If (in_file IsNot Nothing) Then in_file.Close()

        ' Initialize
        in_file = New MemoryStream()

        ' Check if the input string is not an illegal path
        If Dir(TextBox1.Text) = "" Then Exit Sub

        ' Get File Name From The Text Box And Create an File Stream
        hTmp = New FileStream(TextBox1.Text, FileMode.Open)
        szOrigFileName = hTmp.Name
        hTmp.Read(d, 0, 3)
        hTmp.Close()
        If ((d(0) = &H1F) And (d(1) = &H8B)) Then
            hGZFile = FileHandler.GZRead(TextBox1.Text)
            hGZFile.CopyTo(in_file)
            hGZFile.Close()
        Else
            hFile = New FileStream(TextBox1.Text, FileMode.Open)
            hFile.CopyTo(in_file)
            hFile.Close()
        End If

        ' Rewind
        in_file.Seek(0, SeekOrigin.Begin)
        in_file.Read(d, 0, 4)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Parse VGM File 
        ' + Check File Dummy Header 'Vgm '(0x56 0x67 0x6d 0x20)
        If (BytesToText(d, 4) <> "Vgm ") Then in_file.Close() : Exit Sub

        ' + Get VGM File Size -> filelength
        in_file.Read(d, 0, 4)
        filelength = BytesToInt32(d) + 4
        If (filelength > in_file.Length) Then in_file.Close() : Exit Sub
        ListBox1.Items.Add("File length is: " + filelength.ToString + " bytes")

        ''''' Debug Output ''''''
        ' + (Custom) Read VGM File GD3 Tag
        ParseGD3()

        ' + (Custom) Scan Type of VGM Files
        ScanVGM()

        ' + Get VGM YM2151 Clock -> temp (Double), Little Endian Order
        '' in_file.Seek(&H30, SeekOrigin.Begin)
        '' in_file.Read(d, 0, 4)
        '' temp = BytesToInt32(d)
        '' If (temp <> 0) Then
        '' ListBox1.Items.Add("YM2151 Frequency is: " + temp.ToString + " MHz")
        '' Else
        '' in_file.Close()
        '' Exit Sub
        '' End If
        ' Note: YM2151 Frequency Is of little Use To Our Project. So We May Just Skip This.

        ' + Get VGM Data Offset -> temp, And Seek To A New File Position Which Pointing At The Head of Data Segment
        in_file.Seek(&H34, SeekOrigin.Begin)
        in_file.Read(d, 0, 4)
        temp = BytesToInt32(d)
        If (temp = 0) Then temp = 12
        filepos = temp + &H34
        in_file.Seek(filepos, SeekOrigin.Begin)
        ListBox1.Items.Add("Data starts at: " + filepos.ToString)

        delay = 0

        in_file.Read(d, 0, 1) : filepos = filepos + 1

        ' Return Directory and File Name
        pathname = Path.GetDirectoryName(TextBox1.Text)
        filename = Path.GetFileNameWithoutExtension(TextBox1.Text)

        ' MIDI FileStream
        out_file_midi = New FileStream(pathname + "\" + filename + ".mid", FileMode.Create)

        MIDIByteCount = 0
        VoicesCount = 0
        RegisterChanged = False
        ReDim Voices(0)

        ' 8 Channels For YM2151
        For k = 0 To 1
            For frlp = 0 To 7
                Note_Old(k, frlp) = -1
                NoteOn_Old(k, frlp) = False
                KF_old(frlp) = -1
                VoiceID_old(k, frlp) = -1
                VolumeChangeAmount_old(k, frlp) = -1

                Note(k, frlp) = -2
                NoteOn(k, frlp) = False
                KF(frlp) = -2
                VoiceID(k, frlp) = -2
                CurrentVoice(k, frlp).VolumeChangeAmount = -2
            Next

            For frlp = 0 To 2
                SSGNote_Old(k, frlp) = -1
                SSGNoteOn_Old(k, frlp) = False
                SSGVolume_Old(k, frlp) = -1
                SSGNote(k, frlp) = -2
                SSGNoteOn(k, frlp) = False
            Next
        Next

        ' 256 Registers 
        Array.Clear(Registers, 0, Registers.Length)
        TL_Tol = Val(TextBox2.Text)
        MaxVol = 0
        Gain = Val(TextBox3.Text)

        ' Defualt: 120 BPM
        BPM = Val(TextBox4.Text)
        TQN = Val(TextBox5.Text)
        ' PPQN Clock
        ListBox2.Items.Add("Ticks per quarter note = " + TQN.ToString)

        ' Write Headers of MIDI "MThd" and First Data Block
        WriteMIDIHeader()

        ' MIDI BPM Formula
        ' 60,000,000 us (MicroSecond)
        BPM_Period = 60000000
        BPM_Period = BPM_Period / BPM

        ' Set MIDI BPM
        Send_Midi(&HFF, &H51, 3)    'Set BPM
        out_file_midi.WriteByte((BPM_Period And &HFF0000) >> 16) : MIDIByteCount = MIDIByteCount + 1
        out_file_midi.WriteByte((BPM_Period And &HFF00) >> 8) : MIDIByteCount = MIDIByteCount + 1
        out_file_midi.WriteByte(BPM_Period And &HFF) : MIDIByteCount = MIDIByteCount + 1

        For frlp = 0 To 7
            Send_Midi(&HE0 + frlp, 8192 And &H7F, 8192 >> 7)
        Next

        ' Parse VGM Data Blocks
        ParseLoop()

        Send_Midi(&HFF, &H2F, 0)    'End of track

        ' Update MIDI Bytes
        WriteMIDIHeader()   'Do it again to update number of bytes written to the track

        out_file_midi.Close()

        out_file_syx = New FileStream(pathname + "\" + filename + ".syx", FileMode.Create)
        out_file_opm = My.Computer.FileSystem.OpenTextFileWriter(pathname + "\" + filename + ".opm", False, utf8WithoutBom)

        WriteInsts()

        out_file_syx.Close()
        out_file_opm.Close()

        ListBox2.Items.Add("Number of voices found: " + VoicesCount.ToString)
        temp = Int(MaxVol * 1000) / 1000
        ListBox2.Items.Add("Maximum volume was: " + temp.ToString + " out of 127")
        temp = Int(((127 / MaxVol) * Gain) * 1000) / 1000
        ListBox2.Items.Add("Set gain to: " + temp.ToString + " to get best result")
        ListBox2.Items.Add("Conversion complete")

    End Sub

    Private Function KeyCodeToMIDINote_Old(data As Integer) As Integer

        Dim FNum As Integer
        Dim Block As Integer
        Dim NoteVal As Integer

        FNum = data And &HF
        Block = (data And &H70) >> 4

        If (CheckBox1.Checked) Then Block = Block - 1
        'If (Block < 0) Then Block = 0

        If Not ((FNum And &H3) = &H3) Then

            NoteVal = (FNum And &HC) / &H4
            NoteVal = NoteVal * 3 + (FNum And &H3)

            If (NoteVal < 11) Then
                NoteVal = 61 + NoteVal
            Else
                NoteVal = 60
            End If

        Else
            NoteVal = &HFF
            ListBox2.Items.Add("Note value was 255")
        End If

        If NoteVal = &HFF Then
            Return (&HFF)
        Else
            Return (NoteVal + (Block - &H4) * 12)
        End If

    End Function

    Private Function KeyCodeToMIDINote(data As Integer) As Integer

        Dim FNum As Integer
        Dim Octave As Integer
        Dim NoteVal As Integer

        FNum = data And &HF
        Octave = (data And &H70) >> 4

        If (CheckBox1.Checked) Then Octave = Octave - 1
        'If (Block < 0) Then Block = 0

        NoteVal = 0

        Select Case (FNum)
            Case 0
                'C#
                NoteVal = 61
            Case 1
                'D
                NoteVal = 62
            Case 2
                'D#
                NoteVal = 63
            Case 3
                'Undefined
            Case 4
                'E
                NoteVal = 64
            Case 5
                'F
                NoteVal = 65
            Case 6
                'F#
                NoteVal = 66
            Case 7
                'Undefined
            Case 8
                'G
                NoteVal = 67
            Case 9
                'G#
                NoteVal = 68
            Case 10
                'A
                NoteVal = 69
            Case 11
                'Undefined
            Case 12
                'A#
                NoteVal = 70
            Case 13
                'B
                NoteVal = 71
            Case 14
                'C
                NoteVal = 72
            Case 15
                'Undefined

        End Select

        If (NoteVal <> 0) Then
            NoteVal = NoteVal + ((Octave - 4) * 12)
        Else
            ListBox2.Items.Add("Note value was invalid")
        End If

        If (NoteVal < 0) Then
            ListBox2.Items.Add("Note value was less than 0")
            NoteVal = 0
        End If

        Return (NoteVal)

    End Function

    Private Function KeyCodeToMIDINoteYM2203(chip As Integer, ch As Integer) As Integer

        Dim FNum As Double
        Dim Block As Double
        Dim NoteFreq As Double
        Dim NoteVal As Integer

        FNum = ((FNumHigh(chip, ch) And &H7) << 8) + (FNumLow(chip, ch) And &HFF)
        Block = (FNumHigh(chip, ch) >> 3) And &H7

        ' YM2203 Freq Formula
        NoteFreq = (FNum * VGMFile.iFreqChip(chipTag.SND_YM2203)) / (1 << (20 - Block)) / 144.0
        ' MIDI Mapper Offset
        NoteVal = FindMIDINote(NoteFreq) + 9

        Return (NoteVal)

    End Function

    Private Function KeyCodeToMIDINoteYM2608(port As Integer, ch As Integer) As Integer

        Dim FNum As Double
        Dim Block As Double
        Dim NoteFreq As Double
        Dim NoteVal As Integer

        FNum = ((FNumHigh(port, ch) And &H7) << 8) + (FNumLow(port, ch) And &HFF)
        Block = (FNumHigh(port, ch) >> 3) And &H7

        ' YM2608 Freq Formula (Manual Page 24)
        NoteFreq = (FNum * VGMFile.iFreqChip(chipTag.SND_YM2608)) / (1 << (20 - Block)) / 144.0
        ' MIDI Mapper Offset
        NoteVal = FindMIDINote(NoteFreq) + 9

        Return (NoteVal)

    End Function

    Private Function KeyCodeToMIDINoteSSG(chip As Integer, ch As Integer, clk As Double) As Integer

        Dim RegTune As Double
        Dim NoteFreq As Double
        Dim NoteVal As Integer

        ' Get From Coarse Tune and Fine Tune
        RegTune = (SSGTuneCoarse(chip, ch) << 8) + SSGTuneFine(chip, ch)

        If (RegTune = 0) Then RegTune = 1

        ' SSG Freq Formula
        ' Freq = MasterClk / (16 * RegTune)
        NoteFreq = clk / (16.0 * RegTune)

        ' MIDI Mapper Offset
        NoteVal = MapMIDINote(chip, ch, NoteFreq)
        ListBox2.Items.Add("Channel" & (ch + 3 * chip) & ": MIDIKey - " & NoteVal)

        Return (NoteVal)

    End Function

    Private Function FindMIDINote(freq As Double) As Integer
        Dim iNoteCnt As Integer = MIDINoteFreqTable.Length

        For indNote As Integer = 0 To iNoteCnt - 1
            If (freq <= MIDINoteFreqTable(indNote)) Then
                If indNote <> 0 Then
                    If (freq - MIDINoteFreqTable(indNote - 1) < MIDINoteFreqTable(indNote) - freq) Then
                        Return indNote - 1
                    Else
                        Return indNote
                    End If
                Else
                    Return 0
                End If
            End If
        Next

        Return -1
    End Function

    Private Function MapMIDINote(chip As Integer, ch As Integer, freq As Double) As Integer
        Dim NoteKey As Double
        Dim MIDIKey As Integer

        NoteKey = 12.0 * (Log(freq) / Log(2.0)) - 48

        MIDIKey = Max(Min(Int(NoteKey), 127), 0)

        ' Update PitchWheel
        PitchWheel(chip, ch) = &H2000 + (NoteKey - MIDIKey) * 2048

        Return MIDIKey
    End Function

    Private Sub TextBox1_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles TextBox1.DragDrop
        Dim s() As String = e.Data.GetData("FileDrop", False)

        TextBox1.Text = s(0)
    End Sub

    Private Sub TextBox1_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles TextBox1.DragEnter
        e.Effect = DragDropEffects.All
    End Sub

    Private Function BytesToText(b() As Byte, l As Integer) As String

        Dim i As Integer
        Dim st As String

        st = ""
        For i = 0 To l - 1
            st = st + Chr(b(i))
        Next

        BytesToText = st

    End Function

    Private Function BytesToInt32(b() As Byte) As Integer

        Dim v As Integer

        v = b(3)
        v = (v << 8) Or b(2)
        v = (v << 8) Or b(1)
        v = (v << 8) Or b(0)

        BytesToInt32 = v

    End Function

    Private Function BytesToInt24(b() As Byte) As Integer

        Dim v As Integer

        v = b(2)
        v = (v << 8) Or b(1)
        v = (v << 8) Or b(0)

        BytesToInt24 = v

    End Function

    Private Function BytesToInt16(b() As Byte) As Integer

        Dim v As Integer

        v = b(1)
        v = (v << 8) Or b(0)

        BytesToInt16 = v

    End Function

    Private Sub RewindFileStream(fp As FileStream, addr As Integer)

        fp.Seek(addr, SeekOrigin.Begin)

    End Sub

    Private Sub RewindFileStream(fp As MemoryStream, addr As Integer)

        fp.Seek(addr, SeekOrigin.Begin)

    End Sub

    Private Function ReadDword(fp As FileStream) As Integer

        Dim dwBuf(3) As Byte

        fp.Read(dwBuf, 0, 4)

        Return BytesToInt32(dwBuf)

    End Function

    Private Function ReadDword(fp As MemoryStream) As Integer

        Dim dwBuf(3) As Byte

        fp.Read(dwBuf, 0, 4)

        Return BytesToInt32(dwBuf)

    End Function
    Private Function ReadByteAddr(fp As FileStream, addr As Integer) As Byte
        Dim dwBuf(0) As Byte

        RewindFileStream(fp, addr)
        fp.Read(dwBuf, 0, 1)

        Return dwBuf(0)

    End Function

    Private Function ReadDwordAddr(fp As FileStream, addr As Integer) As Integer
        Dim dwBuf(3) As Byte

        RewindFileStream(fp, addr)
        fp.Read(dwBuf, 0, 4)

        Return BytesToInt32(dwBuf)

    End Function

    Private Function ReadDwordAddr(fp As MemoryStream, addr As Integer) As Integer
        Dim dwBuf(3) As Byte

        RewindFileStream(fp, addr)
        fp.Read(dwBuf, 0, 4)

        Return BytesToInt32(dwBuf)

    End Function

    Private Function RetBaseName(pathname As String) As String
        Dim lp As Integer = -1
        Dim rp As Integer = -1
        Dim base As String

        If (pathname.Contains("\")) Then lp = pathname.LastIndexOf("\")
        If (pathname.Contains("/")) Then
            If (pathname.LastIndexOf("/") > lp) Then lp = pathname.LastIndexOf("/")
        End If

        If (pathname.Contains(".")) Then
            rp = pathname.LastIndexOf(".")
            base = pathname.Substring(lp, rp - lp)
        Else
            base = pathname.Substring(lp)
        End If

        Return base

    End Function

    Private Function RemoveFileExt(pathname As String) As String
        Dim rp As Integer = -1
        Dim base As String

        If (pathname.Contains(".")) Then
            rp = pathname.LastIndexOf(".")
            base = pathname.Substring(0, rp)
        Else
            base = pathname
        End If

        Return base

    End Function

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class
