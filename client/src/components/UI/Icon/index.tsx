import { FC } from 'react';
import iconMap from './config/iconMap';

interface IconProps {
  name: keyof typeof iconMap;
  [x: string]: any; // for any additional props passed to object (typescript complains if not added)
}

const Icon: FC<IconProps> = ({ name, ...props }) => {
  const IconComponent = iconMap[name];

  if (!IconComponent) {
    console.assert(false, `Icon component for "${name}" not found.`);
  }

  return <IconComponent {...props}></IconComponent>;
};

export default Icon;
