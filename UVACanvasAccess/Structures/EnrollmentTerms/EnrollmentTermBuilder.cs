using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.EnrollmentTerms {
    
    [PublicAPI]
    public class EnrollmentTermBuilder {
        private string name;
        private DateTime? startAt;
        private DateTime? endAt;
        private string sisTermId;
        private Dictionary<Api.CourseEnrollmentType, EnrollmentTermDateOverride> overrides 
            = new Dictionary<Api.CourseEnrollmentType, EnrollmentTermDateOverride>();

        public EnrollmentTermBuilder WithName(string name) {
            this.name = name;
            return this;
        }

        public EnrollmentTermBuilder WithStartAt(DateTime startAt) {
            this.startAt = startAt;
            return this;
        }

        public EnrollmentTermBuilder WithEndAt(DateTime endAt) {
            this.endAt = endAt;
            return this;
        }

        public EnrollmentTermBuilder WithSisTermId(string sisTermId) {
            this.sisTermId = sisTermId;
            return this;
        }

        public EnrollmentTermBuilder WithOverride(Api.CourseEnrollmentType type, EnrollmentTermDateOverride dateOverride) {
            overrides.Add(type, dateOverride);
            return this;
        }

        internal IEnumerable<(string, string)> ToParams() {
            var args = new List<(string, string)>();
            
            if (name != null) {
                args.Add(("enrollment_term[name]", name));
            }
            
            if (startAt.HasValue) {
                args.Add(("enrollment_term[start_at]", startAt!.ToString()));
            }

            if (endAt.HasValue) {
                args.Add(("enrollment_term[end_at]", endAt!.ToString()));
            }

            if (sisTermId != null) {
                args.Add(("enrollment_term[sis_term_id]", sisTermId));
            }

            foreach (var (courseEnrollmentType, dates) in overrides) {
                if (dates.StartAt.HasValue) {
                    args.Add(($"enrollment_term[overrides][{courseEnrollmentType}][start_at]", dates.StartAt!.ToString()));
                }
                if (dates.EndAt.HasValue) {
                    args.Add(($"enrollment_term[overrides][{courseEnrollmentType}][end_at]", dates.EndAt!.ToString()));
                }
            }

            return args;
        }
    }
}
