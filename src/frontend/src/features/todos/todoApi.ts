export const todoApiSurface = [
  {
    scope: "todos.read",
    behavior: "Allows a signed-in user to list only their own todo resources.",
  },
  {
    scope: "todos.write",
    behavior: "Allows creating and updating todo resources owned by the subject.",
  },
  {
    scope: "profile",
    behavior: "Allows the frontend to show the current demo user's identity and roles.",
  },
] as const;
