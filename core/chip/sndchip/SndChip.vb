Imports VGM2MID.Instrument

Namespace Core
    Namespace SndChip
        Public MustInherit Class SndChip : Inherits IChip
            ' Components
            Protected Reg As Memory
        End Class

        Public MustInherit Class PSGChip : Inherits SndChip
            Public MustOverride Function Dump() As PSGInstrument
        End Class

        Public MustInherit Class FMChip : Inherits SndChip
            Protected iOperatorCount As Integer
            Protected iChannelCount As Integer

            Protected fPortMode As Boolean

            Public Function DumpFM() As FMInstrument
                Dim Instr As FMInstrument = New FMInstrument()
                ReDim Instr.stAttr(iOperatorCount)

                For i As Integer = 0 To iOperatorCount
                    Instr.AR(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.AR)
                    Instr.D1R(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.D1R)
                    Instr.D2R(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.D2R)
                    Instr.SR(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.SR)
                    Instr.RR(i) = Reg.Read(MMap.SndChipMapper.FMMapperID.RR)
                Next

                Return Instr
            End Function
        End Class
    End Namespace
End Namespace