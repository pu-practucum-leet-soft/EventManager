import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@styles": "/src/styles",
      "@components": "/src/components",
      "@config": "/src/config",
      "@queries": "/src/queries",
      "@hooks": "/src/hooks",
      "@utils": "/src/utils",
      "@pages": "/src/pages",
    },
  },
});
