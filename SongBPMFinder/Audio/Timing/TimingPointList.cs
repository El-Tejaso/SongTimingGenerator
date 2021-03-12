using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio.Timing
{
    public class TimingPointList
    {
        public enum TimingFormat
        {
            Osu
        };

        List<TimingPoint> timingPoints;

        public List<TimingPoint> List => timingPoints;

        public int First(double time)
        {
            for(int i = 0; i < timingPoints.Count; i++)
            {
                if (timingPoints[i].OffsetSeconds >= time)
                    return i;
            }
            return timingPoints.Count;
        }

        public static List<TimingPoint> RemoveDebugPoints(List<TimingPoint> timingPoints)
        {
            List<TimingPoint> cleanList = new List<TimingPoint>();

            for(int i = 0; i < timingPoints.Count; i++)
            {
                if(timingPoints[i].Color == Color.Red)
                {
                    cleanList.Add(timingPoints[i]);
                }
            }

            return cleanList;
        }

        public static List<TimingPoint> RemoveDoubles(List<TimingPoint> timingPoints, double tolerance = 0.0001)
        {
            if (timingPoints.Count <= 1) return timingPoints;

            List<TimingPoint> cleanList = new List<TimingPoint>();
            cleanList.Capacity = timingPoints.Count;

            int doubles = 0;
            for (int i = 0; i < timingPoints.Count - 1; i++)
            {
                double a = timingPoints[i].OffsetSeconds;
                double b = timingPoints[i + 1].OffsetSeconds;

                if (Math.Abs(a - b) >= tolerance)
                {
                    cleanList.Add(timingPoints[i]);
                }
                else
                {
                    doubles++;
                }
            }

            return cleanList;
        }

        public static List<TimingPoint> CalculateBpms(List<TimingPoint> timingPoints)
        {
            for(int i = 0; i<timingPoints.Count-1; i++)
            {
                TimingPoint tp0 = timingPoints[i];
                TimingPoint tp1 = timingPoints[i+1];

                double t0 = tp0.OffsetSeconds;
                double t1 = tp1.OffsetSeconds;

                

                double bpm = 60.0/ (t1 - t0);

                tp0.BPM = bpm;

                timingPoints[i] = tp0;
            }

            return timingPoints;
        }

		public static int FindInsertPoint(List<TimingPoint> timingPoints, double s){
			if(timingPoints.Count == 0){
				return 0;
			}

			int pos = 0;

			for(int k = timingPoints.Count-1; k >= 1; k/=2){
				while(pos+k < timingPoints.Count && timingPoints[pos+k].OffsetSeconds <= s){
					pos += k;
				}

				if(k==1)
					break;
			}

			return pos+1;
		}

		public static int FindClosest(List<TimingPoint> timingPoints, double s, double distance){
			int pos = 0;

			double bestDist = s;

			for(int k = timingPoints.Count-1; k >= 1; k/=2){
				while(pos+k < timingPoints.Count){
					double dist = Math.Abs(timingPoints[pos+k].OffsetSeconds-s);

					if(dist < bestDist){
						pos+=k;
						bestDist = dist;
					}
					else
						break;
				}

				if(k==1)
					break;
			}

			return pos;
		}

		public static int InsertSorted(List<TimingPoint> timingPoints, TimingPoint tp){
			int pos = FindInsertPoint(timingPoints, tp.OffsetSeconds);
			timingPoints.Insert(pos, tp);
			return pos;
		}

		//Same as InsertSorted but cache friendly. should be faster if tp will be near the back of the list
		public static int AddSorted(List<TimingPoint> timingPoints, TimingPoint tp)
		{
			int pos = timingPoints.Count;
			timingPoints.Add(tp);

			while(pos > 0 && (timingPoints[pos-1].OffsetSeconds > timingPoints[pos].OffsetSeconds))
			{
				TimingPoint temp = timingPoints[pos-1];
				timingPoints[pos-1] = timingPoints[pos];
				timingPoints[pos] = temp;
				pos--;
			}

			return pos;
		}

		//Combine a new timing point with an existing one if they are close enough
		public static void AddCoalescing(List<TimingPoint> timingPoints, TimingPoint tp, double windowSize)
		{
			if(timingPoints.Count == 0){
				timingPoints.Add(tp);
				return;
			}

			windowSize /= 2.0;

			int pos = FindClosest(timingPoints, tp.OffsetSeconds, windowSize);
			TimingPoint tpPos = timingPoints[pos];
			if(Math.Abs(tpPos.OffsetSeconds - tp.OffsetSeconds) > windowSize){
				AddSorted(timingPoints, tp);
				return;
			}

			
			tpPos.OffsetSeconds = ((tpPos.Weight*tpPos.OffsetSeconds) + tp.OffsetSeconds)/(tpPos.Weight+1);

			tpPos.Weight++;
		}


        public static List<TimingPoint> Simplify(List<TimingPoint> timingPoints, double tolerance)
        {
            if (timingPoints.Count == 0) return timingPoints;

            List<TimingPoint> simplifiedList = new List<TimingPoint>();

            simplifiedList.Add(timingPoints[0]);

            for (int i = 1; i < timingPoints.Count; i++)
            {
                TimingPoint tpPrev = simplifiedList[simplifiedList.Count - 1];
                TimingPoint tpCurr = timingPoints[i];
                if (tpCurr.OffsetSeconds > 3.5)
                {
                    //breakpoint
                }
                
                if(tpPrev.SnapValue(tpCurr.OffsetSeconds, tolerance) == -1)
                {
                    simplifiedList.Add(tpCurr);
                } 
                else if (!tpPrev.IsEquivelantBPM(tpCurr.BPM, tolerance))
                {
                    simplifiedList.Add(tpCurr);
                }
            }

            Logger.Log("Simplified timing points from " + timingPoints.Count + " to " + simplifiedList.Count + " timing points");

            return simplifiedList;
        }

        public void RemoveDoubles(double tolerance = 0.0001){
            int oldCount = timingPoints.Count;
            timingPoints = TimingPointList.RemoveDoubles(timingPoints);

            int doubles = oldCount - timingPoints.Count;
            //if(doubles>0)
            Logger.Log("" + doubles + " duplicate timing points removed");
		}

        public TimingPointList(List<TimingPoint> timingPointsIn, bool clean = true)
        {
            timingPoints = timingPointsIn;
            timingPoints.Sort();
            if(clean)
			    RemoveDoubles();
        }

        public string GetOsuString() {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < timingPoints.Count; i++)
            {
                TimingPoint tp = timingPoints[i];
                //sb.Append("[Offset in MS, DeltaTime in MS(60000 / BPM), TimeSig / 4, 2, Sampleset, volume %, 1, 0]");
                //735,324.324324324324,4,2,22,42,1,0
                sb.Append("" + tp.OffsetMilliseconds + "," + (60000 / tp.BPM) + ",4,2,1,100,1,0\n");
            }

            return sb.ToString();
        }

        public string ToString(TimingFormat format)
        {
            switch (format)
            {
                default:
                {
                    return GetOsuString();
                }
            }
        }
    }
}
