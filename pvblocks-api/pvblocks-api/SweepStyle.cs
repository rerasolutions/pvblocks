namespace pvblocks_api;

public enum SweepStyle
{
    ISC_TO_VOC = 0,
    SWEEP_VOC_TO_ISC = 1,
    EXTENT_CURVE_DELAY = 2,
    SWEEP_VOC_ISC_VOC = 4,
    SWEEP_ISC_VOC_ISC = 8,
    SWEEP_FAST_PMAX = 16

}