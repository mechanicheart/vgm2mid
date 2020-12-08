Imports VGM2MID.Instrument

Namespace Core
    Namespace SndChip
        Public MustInherit Class SndChip : Inherits IChip
            ' Components
            Protected Reg As Memory

            Public Sub Write(devid As Integer, addr As Integer, data As Byte)
                Reg.Write(addr, data)
            End Sub

        End Class

        Public MustInherit Class PSGChip : Inherits SndChip
            Public MustOverride Function Dump() As PSGInstrument
        End Class

        Public MustInherit Class FMChip : Inherits SndChip
            Protected iOperatorCount As Integer
            Protected iChannelCount As Integer

            Protected fPortSupport As Boolean

            Public Function DumpFM(chan As Integer) As FMInstrument
                Dim Instr As FMInstrument = New FMInstrument()
                ReDim Instr.FMInstrOp(iOperatorCount)

                For i As Integer = 0 To iOperatorCount
                    Instr.AR(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.AR, chan, i)
                    Instr.D1R(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.D1R, chan, i)
                    Instr.D2R(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.D2R, chan, i)
                    Instr.SR(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.SR, chan, i)
                    Instr.RR(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.RR, chan, i)
                Next

                Return Instr
            End Function
        End Class
    End Namespace
End Namespace