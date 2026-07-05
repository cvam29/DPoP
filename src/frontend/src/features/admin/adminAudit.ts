export const adminAuditChecks = [
  {
    name: "Role gate",
    rule: "Only tokens with the admin role can read audit events.",
  },
  {
    name: "Scope gate",
    rule: "The admin.audit scope is required in addition to the role.",
  },
  {
    name: "Audit trail",
    rule: "Sensitive auth events are stored for portfolio inspection.",
  },
] as const;
