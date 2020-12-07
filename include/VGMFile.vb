Imports System.IO

Public Class VGMFile
    ' 256 Files (Maximum)
    Dim hFileList(255) As FileStream
    ' Current VGM Version (1.71 Beta)
    Dim iVGMVersion As Integer = 171
    Dim szChipTag As String() = New String(63) {
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
    Public Enum iVGMChipID As Integer
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

    Public Sub open(file As String())
        Dim i As Integer

        For i = 0 To file.Length()
            hFileList(i) = New FileStream(file(i), FileMode.Open)
        Next

    End Sub

    Public Function readChipTag(id As iVGMChipID) As String
        Return szChipTag(id)
    End Function

End Class