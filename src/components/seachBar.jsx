export default function SearchBar({ value, onChange }) {
  return (
    <input
      type="text"
      className="search-input"
      placeholder="Pesquisar..."
      value={value}
      onChange={(e) => onChange(e.target.value)}
    />
  );
}
