```css
:root {
  /* Colors */
  --primary-color: #3b82f6;
  --primary-hover: #2563eb;
  --primary-light: #eff6ff;

  --secondary-color: #64748b;
  --secondary-hover: #475569;

  --success-color: #22c55e;
  --success-hover: #16a34a;

  --danger-color: #ef4444;
  --danger-hover: #dc2626;

  --warning-color: #f59e0b;
  --warning-hover: #d97706;

  --background-color: #f8fafc;
  --surface-color: #ffffff;

  --text-primary: #1e293b;
  --text-secondary: #64748b;
  --text-muted: #94a3b8;
  --text-on-primary: #ffffff;

  --border-color: #e2e8f0;
  --border-focus: #3b82f6;

  /* Spacing */
  --spacing-xs: 4px;
  --spacing-sm: 8px;
  --spacing-md: 16px;
  --spacing-lg: 24px;
  --spacing-xl: 32px;

  /* Radius */
  --radius-sm: 4px;
  --radius-md: 6px;
  --radius-lg: 8px;

  /* Shadow */
  --shadow-sm: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
  --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);

  /* Typography */
  --font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
  --font-size-base: 16px;
  --line-height-base: 1.5;
}

* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  font-family: var(--font-family);
  font-size: var(--font-size-base);
  line-height: var(--line-height-base);
  color: var(--text-primary);
  background-color: var(--background-color);
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

#app {
  min-height: 100vh;
}

/* Typography */
h1,
h2,
h3,
h4,
h5,
h6 {
  color: var(--text-primary);
  font-weight: 700;
  line-height: 1.2;
  margin-bottom: var(--spacing-sm);
}

a {
  color: var(--primary-color);
  text-decoration: none;
  transition: color 0.2s;
}

a:hover {
  color: var(--primary-hover);
}

/* Common Elements */
button {
  font-family: inherit;
  font-size: 0.875rem;
  font-weight: 500;
  padding: 8px 16px;
  border-radius: var(--radius-md);
  border: 1px solid transparent;
  cursor: pointer;
  transition: all 0.2s;
}

button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Form Elements */
input,
select,
textarea {
  font-family: inherit;
  font-size: 1rem;
  padding: 8px 12px;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  background-color: var(--surface-color);
  color: var(--text-primary);
  width: 100%;
  transition: border-color 0.2s, box-shadow 0.2s;
}

input:focus,
select:focus,
textarea:focus {
  outline: none;
  border-color: var(--border-focus);
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

label {
  display: block;
  font-size: 0.875rem;
  font-weight: 600;
  color: var(--text-secondary);
  margin-bottom: var(--spacing-xs);
}

/* Card */
.card {
  background-color: var(--surface-color);
  border-radius: var(--radius-lg);
  border: 1px solid var(--border-color);
  box-shadow: var(--shadow-sm);
  padding: var(--spacing-md);
}

/* Table */
table {
  width: 100%;
  border-collapse: collapse;
}

th {
  text-align: left;
  padding: 12px 16px;
  border-bottom: 2px solid var(--border-color);
  color: var(--text-secondary);
  font-weight: 600;
  font-size: 0.875rem;
}

td {
  padding: 12px 16px;
  border-bottom: 1px solid var(--border-color);
  font-size: 0.875rem;
}

tr:hover td {
  background-color: var(--primary-light);
}
```
